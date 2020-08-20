using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Serilog;
using TheDialgaTeam.Core.DependencyInjection;
using YouTubePlays.Discord.Bot.Config;

namespace YouTubePlays.Discord.Bot.Discord
{
    public class DiscordAppServiceExecutor : IServiceExecutor, IDisposable
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        private readonly DiscordShardedClient _discordClient;

        public DiscordAppServiceExecutor(ILogger logger, IConfig config, CancellationTokenSource cancellationTokenSource, CommandService commandService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _cancellationTokenSource = cancellationTokenSource;
            _commandService = commandService;
            _serviceProvider = serviceProvider;

            _discordClient = new DiscordShardedClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            _discordClient.Log += DiscordClientOnLog;
            _discordClient.ShardReady += DiscordClientOnShardReady;
            _discordClient.MessageReceived += DiscordClientOnMessageReceived;
        }

        public void ExecuteService(ITaskCreator taskCreator)
        {
            taskCreator.CreateAndEnqueueTask((_logger, _config, _discordClient), async (state, cancellationToken) =>
            {
                var (logger, config, discordClient) = state;

                await discordClient.LoginAsync(TokenType.Bot, config.BotToken).ConfigureAwait(false);
                await discordClient.StartAsync().ConfigureAwait(false);

                while (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken).ConfigureAwait(false);

                    try
                    {
                        if (discordClient.LoginState == LoginState.LoggingOut || discordClient.LoginState == LoginState.LoggedOut)
                        {
                            await discordClient.LoginAsync(TokenType.Bot, config.BotToken).ConfigureAwait(false);
                        }
                    }
                    catch (Exception ex)
                    {
                        logger.Error(ex, "\u001b[31;1mUnable to authenticate into discord server.\u001b[0m");
                    }

                    foreach (var discordSocketClient in discordClient.Shards)
                    {
                        try
                        {
                            if (discordSocketClient.ConnectionState == ConnectionState.Disconnected || discordSocketClient.ConnectionState == ConnectionState.Disconnecting)
                            {
                                await discordSocketClient.StartAsync().ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error(ex, "\u001b[31;1mUnable to connect into discord server.\u001b[0m");
                        }
                    }
                }

                await discordClient.LogoutAsync().ConfigureAwait(false);
                await discordClient.StopAsync().ConfigureAwait(false);
            });
        }

        private Task DiscordClientOnLog(LogMessage logMessage)
        {
            return Task.Factory.StartNew(state =>
            {
                if (!(state is (ILogger logger, DiscordShardedClient discordShardedClient, LogMessage logMessageState))) return;

                var currentUser = discordShardedClient.CurrentUser;
                var message = currentUser == null ? $"[Bot] {logMessageState.ToString()}" : $"[Bot {currentUser.Id}] {currentUser.Username}: {logMessageState.ToString()}";

                switch (logMessageState.Severity)
                {
                    case LogSeverity.Critical:
                        logger.Fatal(message);
                        break;

                    case LogSeverity.Error:
                        logger.Error(message);
                        break;

                    case LogSeverity.Warning:
                        logger.Warning(message);
                        break;

                    case LogSeverity.Info:
                        logger.Information(message);
                        break;

                    case LogSeverity.Verbose:
                        logger.Verbose(message);
                        break;

                    case LogSeverity.Debug:
                        logger.Debug(message);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, (_logger, _discordClient, logMessage), _cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private Task DiscordClientOnShardReady(DiscordSocketClient discordSocketClient)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (ILogger logger, DiscordShardedClient discordShardedClient, DiscordSocketClient discordSocketClientState))
                {
                    var currentUser = discordSocketClientState.CurrentUser;
                    await discordSocketClientState.SetGameAsync($"{currentUser.Username} help").ConfigureAwait(false);
                    logger.Information($"[Bot {currentUser.Id}] {currentUser.Username}: \u001b[32;1mShard {discordSocketClientState.ShardId + 1}/{discordShardedClient.Shards.Count} is ready!\u001b[0m");
                }
            }, (_logger, _discordClient, discordSocketClient), _cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }

        private Task DiscordClientOnMessageReceived(SocketMessage socketMessage)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (IConfig config, DiscordShardedClient discordShardedClient, CommandService commandService, IServiceProvider serviceProvider, SocketMessage message))
                {
                    if (message is SocketUserMessage socketUserMessage)
                    {
                        ICommandContext context = new ShardedCommandContext(discordShardedClient, socketUserMessage);
                        var argPos = 0;

                        if (socketUserMessage.Channel is SocketDMChannel)
                        {
                            socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos);
                        }
                        else
                        {
                            if (!socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos) &&
                                !socketUserMessage.HasStringPrefix(config.BotPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }

                        await commandService.ExecuteAsync(context, argPos, serviceProvider).ConfigureAwait(false);
                    }
                }
            }, (_config, _discordClient, _commandService, _serviceProvider, socketMessage), _cancellationTokenSource.Token, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }

        public void Dispose()
        {
            _discordClient.Dispose();
        }
    }
}
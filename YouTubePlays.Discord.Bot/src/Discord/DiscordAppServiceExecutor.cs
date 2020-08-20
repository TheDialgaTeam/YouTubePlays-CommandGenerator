using System;
using System.Threading;
using System.Threading.Tasks;
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
        private readonly CancellationToken _cancellationToken;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;

        private readonly DiscordShardedClient _discordClient;

        public DiscordAppServiceExecutor(ILogger logger, IConfig config, CancellationTokenSource cancellationTokenSource, CommandService commandService, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _config = config;
            _cancellationToken = cancellationTokenSource.Token;
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

                switch (logMessageState.Severity)
                {
                    case LogSeverity.Critical:
                        if (currentUser == null)
                        {
                            logger.Fatal("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.Fatal("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    case LogSeverity.Error:
                        if (currentUser == null)
                        {
                            logger.Error("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.Error("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    case LogSeverity.Warning:
                        if (currentUser == null)
                        {
                            logger.Warning("[Bot] \u001b[43;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.Warning("[Bot {id}] {Username:l}: \u001b[43;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    case LogSeverity.Info:
                        if (currentUser == null)
                        {
                            logger.Information("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.Information("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    case LogSeverity.Verbose:
                        if (currentUser == null)
                        {
                            logger.Verbose("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.Verbose("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    case LogSeverity.Debug:
                        if (currentUser == null)
                        {
                            logger.Debug("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.Debug("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
                        }
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, (_logger, _discordClient, logMessage), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        private Task DiscordClientOnShardReady(DiscordSocketClient discordSocketClient)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (ILogger logger, DiscordShardedClient discordShardedClient, DiscordSocketClient discordSocketClientState))
                {
                    var currentUser = discordSocketClientState.CurrentUser;
                    await discordSocketClientState.SetGameAsync($"{currentUser.Username} help").ConfigureAwait(false);

                    logger.Information("[Bot {Id}] {Username:l}: \u001b[32;1mShard {CurrentShard}/{TotalShard} is ready!\u001b[0m", currentUser.Id, currentUser.Username, discordSocketClientState.ShardId + 1, discordShardedClient.Shards.Count);
                }
            }, (_logger, _discordClient, discordSocketClient), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
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
            }, (_config, _discordClient, _commandService, _serviceProvider, socketMessage), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }

        public void Dispose()
        {
            _discordClient.Dispose();
        }
    }
}
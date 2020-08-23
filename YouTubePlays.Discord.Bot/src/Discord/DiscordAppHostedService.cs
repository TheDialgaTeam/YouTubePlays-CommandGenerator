using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YouTubePlays.Discord.Bot.Discord
{
    public class DiscordAppHostedService : IHostedService, IDisposable
    {
        private readonly IConfiguration _configuration;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<DiscordAppHostedService> _logger;
        private readonly CancellationToken _cancellationToken;

        private readonly DiscordShardedClient _discordClient;
        private Task? _checkConnectionTask;

        public DiscordAppHostedService(IConfiguration configuration, CommandService commandService, IServiceProvider serviceProvider, ILogger<DiscordAppHostedService> logger, CancellationTokenSource cancellationTokenSource)
        {
            _configuration = configuration;
            _commandService = commandService;
            _serviceProvider = serviceProvider;
            _logger = logger;
            _cancellationToken = cancellationTokenSource.Token;

            _discordClient = new DiscordShardedClient(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });
            _discordClient.Log += DiscordClientOnLog;
            _discordClient.ShardReady += DiscordClientOnShardReady;
            _discordClient.MessageReceived += DiscordClientOnMessageReceived;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var botToken = _configuration.GetValue<string>("Discord:BotToken");

            await _discordClient.LoginAsync(TokenType.Bot, botToken).ConfigureAwait(false);
            await _discordClient.StartAsync().ConfigureAwait(false);

            _checkConnectionTask = Task.Factory.StartNew(async state =>
            {
                if (state is (DiscordShardedClient discordShardedClient, ILogger<DiscordAppHostedService> logger, string botTokenState, CancellationToken cancellationTokenState))
                {
                    while (!cancellationTokenState.IsCancellationRequested)
                    {
                        await Task.Delay(TimeSpan.FromMinutes(15), cancellationToken).ConfigureAwait(false);

                        try
                        {
                            if (discordShardedClient.LoginState == LoginState.LoggingOut || discordShardedClient.LoginState == LoginState.LoggedOut)
                            {
                                await discordShardedClient.LoginAsync(TokenType.Bot, botTokenState).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "\u001b[31;1mUnable to authenticate into discord server.\u001b[0m");
                        }

                        foreach (var discordSocketClient in discordShardedClient.Shards)
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
                                logger.LogError(ex, "\u001b[31;1mUnable to connect into discord server.\u001b[0m");
                            }
                        }
                    }
                }
            }, (_discordClient, _logger, botToken, cancellationToken), cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _discordClient.LogoutAsync().ConfigureAwait(false);
            await _discordClient.StopAsync().ConfigureAwait(false);
        }

        private Task DiscordClientOnLog(LogMessage logMessage)
        {
            return Task.Factory.StartNew(state =>
            {
                if (!(state is (ILogger<DiscordAppHostedService> logger, DiscordShardedClient discordShardedClient, LogMessage logMessageState))) return;

                var currentUser = discordShardedClient.CurrentUser;

                switch (logMessageState.Severity)
                {
                    case LogSeverity.Critical:
                        if (currentUser == null)
                        {
                            logger.LogCritical("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.LogCritical("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }

                        break;

                    case LogSeverity.Error:
                        if (currentUser == null)
                        {
                            logger.LogError("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.LogError("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }

                        break;

                    case LogSeverity.Warning:
                        if (currentUser == null)
                        {
                            logger.LogWarning("[Bot] \u001b[33;1m{logMessageState:l}\u001b[0m", logMessageState);
                        }
                        else
                        {
                            logger.LogWarning("[Bot {id}] {Username:l}: \u001b[33;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessageState);
                        }

                        break;

                    case LogSeverity.Info:
                        if (currentUser == null)
                        {
                            logger.LogInformation("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.LogInformation("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
                        }

                        break;

                    case LogSeverity.Verbose:
                        if (currentUser == null)
                        {
                            logger.LogTrace("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.LogTrace("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
                        }

                        break;

                    case LogSeverity.Debug:
                        if (currentUser == null)
                        {
                            logger.LogDebug("[Bot] {logMessageState:l}", logMessageState);
                        }
                        else
                        {
                            logger.LogDebug("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessageState);
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
                if (state is (ILogger<DiscordAppHostedService> logger, DiscordShardedClient discordShardedClient, DiscordSocketClient discordSocketClientState))
                {
                    var currentUser = discordSocketClientState.CurrentUser;
                    await discordSocketClientState.SetGameAsync($"{currentUser.Username} help").ConfigureAwait(false);

                    logger.LogInformation("[Bot {Id}] {Username:l}: \u001b[32;1mShard {CurrentShard}/{TotalShard} is ready!\u001b[0m", currentUser.Id, currentUser.Username, discordSocketClientState.ShardId + 1, discordShardedClient.Shards.Count);
                }
            }, (_logger, _discordClient, discordSocketClient), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }

        private Task DiscordClientOnMessageReceived(SocketMessage socketMessage)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (IConfiguration configuration, DiscordShardedClient discordShardedClient, CommandService commandService, IServiceProvider serviceProvider, SocketMessage message))
                {
                    if (message is SocketUserMessage socketUserMessage)
                    {
                        ICommandContext context = new ShardedCommandContext(discordShardedClient, socketUserMessage);
                        var argPos = 0;
                        var botPrefix = configuration.GetValue<string>("Discord:BotPrefix");

                        if (socketUserMessage.Channel is SocketDMChannel)
                        {
                            if (socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos) ||
                                socketUserMessage.HasStringPrefix(botPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                            {
                            }
                        }
                        else
                        {
                            if (!socketUserMessage.HasMentionPrefix(discordShardedClient.CurrentUser, ref argPos) &&
                                !socketUserMessage.HasStringPrefix(botPrefix, ref argPos, StringComparison.OrdinalIgnoreCase))
                            {
                                return;
                            }
                        }

                        await commandService.ExecuteAsync(context, argPos, serviceProvider).ConfigureAwait(false);
                    }
                }
            }, (_configuration, _discordClient, _commandService, _serviceProvider, socketMessage), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }

        public void Dispose()
        {
            _checkConnectionTask?.Dispose();
        }
    }
}
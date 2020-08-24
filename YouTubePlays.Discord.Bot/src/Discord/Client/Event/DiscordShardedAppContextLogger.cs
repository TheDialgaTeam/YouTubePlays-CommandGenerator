using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YouTubePlays.Discord.Bot.Discord.Client.Event
{
    public class DiscordShardedAppContextLogger : IDiscordShardedAppContextEvent
    {
        private readonly ILogger<DiscordShardedAppContextLogger> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public DiscordShardedAppContextLogger(ILogger<DiscordShardedAppContextLogger> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public void Add(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.Log += DiscordShardedClientOnLog;
        }

        public void Remove(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.Log -= DiscordShardedClientOnLog;
        }

        private Task DiscordShardedClientOnLog(DiscordShardedClient arg1, LogMessage arg2)
        {
            return Task.Factory.StartNew(state =>
            {
                if (!(state is (ILogger<DiscordShardedAppContextLogger> logger, DiscordShardedClient discordShardedClient, LogMessage logMessage))) return;

                var currentUser = discordShardedClient.CurrentUser;

                switch (logMessage.Severity)
                {
                    case LogSeverity.Critical:
                        if (currentUser == null)
                        {
                            logger.LogCritical("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessage);
                        }
                        else
                        {
                            logger.LogCritical("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    case LogSeverity.Error:
                        if (currentUser == null)
                        {
                            logger.LogError("[Bot] \u001b[31;1m{logMessageState:l}\u001b[0m", logMessage);
                        }
                        else
                        {
                            logger.LogError("[Bot {id}] {Username:l}: \u001b[31;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    case LogSeverity.Warning:
                        if (currentUser == null)
                        {
                            logger.LogWarning("[Bot] \u001b[33;1m{logMessageState:l}\u001b[0m", logMessage);
                        }
                        else
                        {
                            logger.LogWarning("[Bot {id}] {Username:l}: \u001b[33;1m{logMessageState:l}\u001b[0m", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    case LogSeverity.Info:
                        if (currentUser == null)
                        {
                            logger.LogInformation("[Bot] {logMessageState:l}", logMessage);
                        }
                        else
                        {
                            logger.LogInformation("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    case LogSeverity.Verbose:
                        if (currentUser == null)
                        {
                            logger.LogTrace("[Bot] {logMessageState:l}", logMessage);
                        }
                        else
                        {
                            logger.LogTrace("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    case LogSeverity.Debug:
                        if (currentUser == null)
                        {
                            logger.LogDebug("[Bot] {logMessageState:l}", logMessage);
                        }
                        else
                        {
                            logger.LogDebug("[Bot {id}] {Username:l}: {logMessageState:l}", currentUser.Id, currentUser.Username, logMessage);
                        }

                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }, (_logger, arg1, arg2), _hostApplicationLifetime.ApplicationStopping, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }
    }
}
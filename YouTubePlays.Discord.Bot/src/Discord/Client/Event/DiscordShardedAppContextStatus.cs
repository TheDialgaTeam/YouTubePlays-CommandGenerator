using System.Threading.Tasks;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace YouTubePlays.Discord.Bot.Discord.Client.Event
{
    public class DiscordShardedAppContextStatus : IDiscordShardedAppContextEvent
    {
        private readonly ILogger<DiscordShardedAppContextLogger> _logger;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;

        public DiscordShardedAppContextStatus(ILogger<DiscordShardedAppContextLogger> logger, IHostApplicationLifetime hostApplicationLifetime)
        {
            _logger = logger;
            _hostApplicationLifetime = hostApplicationLifetime;
        }

        public void Add(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.ShardReady += DiscordShardedAppContextOnShardReady;
        }

        public void Remove(DiscordShardedAppContext discordShardedAppContext)
        {
            discordShardedAppContext.ShardReady -= DiscordShardedAppContextOnShardReady;
        }

        private Task DiscordShardedAppContextOnShardReady(DiscordShardedClient arg1, DiscordSocketClient arg2)
        {
            return Task.Factory.StartNew(async state =>
            {
                if (state is (ILogger<DiscordShardedAppContextStatus> logger, DiscordShardedClient discordShardedClient, DiscordSocketClient discordSocketClient))
                {
                    var currentUser = discordSocketClient.CurrentUser;
                    await discordSocketClient.SetGameAsync($"{currentUser.Username} help").ConfigureAwait(false);

                    logger.LogInformation("[Bot {Id}] {Username:l}: \u001b[32;1mShard {CurrentShard}/{TotalShard} is ready!\u001b[0m", currentUser.Id, currentUser.Username, discordSocketClient.ShardId + 1, discordShardedClient.Shards.Count);
                }
            }, (_logger, arg1, arg2), _hostApplicationLifetime.ApplicationStopping, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap();
        }
    }
}
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using YouTubePlays.Discord.Bot.Discord.Client;

namespace YouTubePlays.Discord.Bot.Discord
{
    public class DiscordAppHostedService : IHostedService
    {
        private readonly DiscordShardedAppContext _discordShardedAppContext;

        public DiscordAppHostedService(DiscordShardedAppContext discordShardedAppContext)
        {
            _discordShardedAppContext = discordShardedAppContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return _discordShardedAppContext.StartAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _discordShardedAppContext.StopAsync();
        }
    }
}
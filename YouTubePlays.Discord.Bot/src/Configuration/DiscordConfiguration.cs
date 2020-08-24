using Microsoft.Extensions.Configuration;

namespace YouTubePlays.Discord.Bot.Configuration
{
    public class DiscordConfiguration
    {
        public string BotToken { get; }

        public string BotPrefix { get; }

        public DiscordConfiguration(IConfiguration configuration)
        {
            BotToken = configuration.GetValue<string>("Discord:BotToken");
            BotPrefix = configuration.GetValue<string>("Discord:BotPrefix");
        }
    }
}
namespace YouTubePlays.Discord.Bot.Discord.Client.Event
{
    public interface IDiscordShardedAppContextEvent
    {
        void Add(DiscordShardedAppContext discordShardedAppContext);

        void Remove(DiscordShardedAppContext discordShardedAppContext);
    }
}
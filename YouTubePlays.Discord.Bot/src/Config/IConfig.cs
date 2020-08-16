namespace YouTubePlays.Discord.Bot.Config
{
    public interface IConfig
    {
        string BotToken { get; }

        string BotPrefix { get; }

        string ConfigFilePath { get; }

        void LoadConfig();

        void SaveConfig();
    }
}
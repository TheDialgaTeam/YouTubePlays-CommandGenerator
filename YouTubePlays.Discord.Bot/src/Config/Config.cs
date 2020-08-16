using System;
using Newtonsoft.Json;

namespace YouTubePlays.Discord.Bot.Config
{
    public abstract class Config : IConfig, IDisposable
    {
        public string BotToken { get; set; } = "";

        public string BotPrefix { get; set; } = "~";

        [JsonIgnore]
        public string ConfigFilePath { get; }

        protected Config(string configFilePath)
        {
            ConfigFilePath = configFilePath;
        }

        public abstract void LoadConfig();

        public abstract void SaveConfig();

        public void Dispose()
        {
            SaveConfig();
        }
    }
}
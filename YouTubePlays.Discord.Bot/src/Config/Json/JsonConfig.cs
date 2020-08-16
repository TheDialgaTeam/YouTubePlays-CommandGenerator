using System.IO;
using Newtonsoft.Json;

namespace YouTubePlays.Discord.Bot.Config.Json
{
    public class JsonConfig : Config
    {
        private readonly JsonSerializer _jsonSerializer;

        public JsonConfig(string configFilePath) : base(configFilePath)
        {
            _jsonSerializer = JsonSerializer.Create(new JsonSerializerSettings { Formatting = Formatting.Indented });
        }

        public override void LoadConfig()
        {
            using var streamReader = new StreamReader(new FileStream(ConfigFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            _jsonSerializer.Populate(new JsonTextReader(streamReader), this);
        }

        public override void SaveConfig()
        {
            using var streamWriter = new StreamWriter(new FileStream(ConfigFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite));
            _jsonSerializer.Serialize(streamWriter, this);
        }
    }
}
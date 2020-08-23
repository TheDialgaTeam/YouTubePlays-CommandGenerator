using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YouTubePlays.Discord.Bot.EntityFramework.Table
{
    public class ChannelSettings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong ChannelId { get; set; }

        public string KeyboardType { get; set; } = "1";

        public int InputLimit { get; set; } = 20;

        public bool TouchAvailable { get; set; } = true;

        public int TouchXOffset { get; set; }

        public int TouchYOffset { get; set; }
    }
}
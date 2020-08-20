namespace YouTubePlays.Discord.Bot
{
    public class ChatBot
    {
        public int InputLimit { get; set; } = 20;

        public bool TouchAvailable { get; set; } = true;

        public int TouchXOffset { get; set; }

        public int TouchYOffset { get; set; }
    }
}
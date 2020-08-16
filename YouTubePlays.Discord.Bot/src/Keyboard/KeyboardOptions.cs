namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class KeyboardOptions
    {
        public (int x, int y)[] KeyMapSizes { get; set; } = { (0, 0) };

        public string PreExecuteCommand { get; set; } = "";

        public (int x, int y) StartingPosition { get; set; } = (0, 0);

        public string ModeSwitchCommand { get; set; } = "sc";

        public int ModeSwitchDelay { get; set; }
    }
}
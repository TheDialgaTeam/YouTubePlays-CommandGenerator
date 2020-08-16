namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class KeyboardOptions
    {
        public (int x, int y)[] KeyMapSizes { get; set; } = { (0, 0) };

        public string PreExecuteCommand { get; set; } = "";

        public string PostExecuteCommand { get; set; } = "st,a";

        public (int x, int y) StartingPosition { get; set; } = (0, 0);

        public int ModeSwitchDelay { get; set; }

        public string ModeSwitchCommand { get; set; } = "sc";

        public string PostModeSwitchCommand { get; set; } = "";

        public (int x, int y)[] PostModeSwitchPosition { get; set; } = { (-1, -1) };
    }
}
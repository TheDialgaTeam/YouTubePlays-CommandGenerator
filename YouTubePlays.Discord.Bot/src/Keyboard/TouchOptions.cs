namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class TouchOptions
    {
        public bool TouchAvailable { get; set; }

        public string PreExecuteCommand { get; set; } = "";

        public (int x, int y)[] ModeSwitchButton { get; set; } = { (-1, -1) };
    }
}
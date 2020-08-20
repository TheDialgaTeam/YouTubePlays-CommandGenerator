namespace YouTubePlays.Discord.Bot.Keyboard.Options
{
    public class TouchOptions
    {
        public bool TouchAvailable { get; set; }

        public string PreExecuteCommand { get; set; } = "";

        public string PostExecuteCommand { get; set; } = "st,a";

        public (int x, int y)[] ModeSwitchButton { get; set; } = { (-1, -1) };
    }
}
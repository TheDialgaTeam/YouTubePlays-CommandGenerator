namespace YouTubePlays.Discord.Bot.Keyboard
{
    public readonly struct KeyMapping
    {
        public int Mode { get; }

        public int X { get; }

        public int Y { get; }

        public int TouchX { get; }

        public int TouchY { get; }

        public KeyMapping(int mode, int x, int y, int touchX = -1, int touchY = -1)
        {
            Mode = mode;
            X = x;
            Y = y;
            TouchX = touchX;
            TouchY = touchY;
        }
    }
}
using System.Collections.Generic;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class Generation2Keyboard : Keyboard
    {
        public override string Name { get; } = "Generation 2 (Gold, Silver and Crystal)";

        public override string ShortKey { get; } = "2";

        public override KeyboardOptions KeyboardOptions { get; } = new KeyboardOptions
        {
            KeyMapSizes = new[] { (8, 4), (8, 4) },
            PreExecuteCommand = "st,d",
            StartingPosition = (8, 0),
            PostModeSwitchPosition = new[] { (-1, -1), (-1, -1) }
        };

        public override TouchOptions TouchOptions { get; } = new TouchOptions
        {
            TouchAvailable = false
        };

        public override int NameLength { get; } = 10;

        public override Dictionary<string, KeyMapping[]> CharMappings { get; } = new Dictionary<string, KeyMapping[]>
        {
            { "A", new[] { new KeyMapping(0, 0, 0) } },
            { "B", new[] { new KeyMapping(0, 1, 0) } },
            { "C", new[] { new KeyMapping(0, 2, 0) } },
            { "D", new[] { new KeyMapping(0, 3, 0) } },
            { "E", new[] { new KeyMapping(0, 4, 0) } },
            { "F", new[] { new KeyMapping(0, 5, 0) } },
            { "G", new[] { new KeyMapping(0, 6, 0) } },
            { "H", new[] { new KeyMapping(0, 7, 0) } },
            { "I", new[] { new KeyMapping(0, 8, 0) } },
            { "J", new[] { new KeyMapping(0, 0, 1) } },
            { "K", new[] { new KeyMapping(0, 1, 1) } },
            { "L", new[] { new KeyMapping(0, 2, 1) } },
            { "M", new[] { new KeyMapping(0, 3, 1) } },
            { "N", new[] { new KeyMapping(0, 4, 1) } },
            { "O", new[] { new KeyMapping(0, 5, 1) } },
            { "P", new[] { new KeyMapping(0, 6, 1) } },
            { "Q", new[] { new KeyMapping(0, 7, 1) } },
            { "R", new[] { new KeyMapping(0, 8, 1) } },
            { "S", new[] { new KeyMapping(0, 0, 2) } },
            { "T", new[] { new KeyMapping(0, 1, 2) } },
            { "U", new[] { new KeyMapping(0, 2, 2) } },
            { "V", new[] { new KeyMapping(0, 3, 2) } },
            { "W", new[] { new KeyMapping(0, 4, 2) } },
            { "X", new[] { new KeyMapping(0, 5, 2) } },
            { "Y", new[] { new KeyMapping(0, 6, 2) } },
            { "Z", new[] { new KeyMapping(0, 7, 2) } },

            { "a", new[] { new KeyMapping(1, 0, 0) } },
            { "b", new[] { new KeyMapping(1, 1, 0) } },
            { "c", new[] { new KeyMapping(1, 2, 0) } },
            { "d", new[] { new KeyMapping(1, 3, 0) } },
            { "e", new[] { new KeyMapping(1, 4, 0) } },
            { "f", new[] { new KeyMapping(1, 5, 0) } },
            { "g", new[] { new KeyMapping(1, 6, 0) } },
            { "h", new[] { new KeyMapping(1, 7, 0) } },
            { "i", new[] { new KeyMapping(1, 8, 0) } },
            { "j", new[] { new KeyMapping(1, 0, 1) } },
            { "k", new[] { new KeyMapping(1, 1, 1) } },
            { "l", new[] { new KeyMapping(1, 2, 1) } },
            { "m", new[] { new KeyMapping(1, 3, 1) } },
            { "n", new[] { new KeyMapping(1, 4, 1) } },
            { "o", new[] { new KeyMapping(1, 5, 1) } },
            { "p", new[] { new KeyMapping(1, 6, 1) } },
            { "q", new[] { new KeyMapping(1, 7, 1) } },
            { "r", new[] { new KeyMapping(1, 8, 1) } },
            { "s", new[] { new KeyMapping(1, 0, 2) } },
            { "t", new[] { new KeyMapping(1, 1, 2) } },
            { "u", new[] { new KeyMapping(1, 2, 2) } },
            { "v", new[] { new KeyMapping(1, 3, 2) } },
            { "w", new[] { new KeyMapping(1, 4, 2) } },
            { "x", new[] { new KeyMapping(1, 5, 2) } },
            { "y", new[] { new KeyMapping(1, 6, 2) } },
            { "z", new[] { new KeyMapping(1, 7, 2) } },

            { "-", new[] { new KeyMapping(0, 0, 3) } },
            { "?", new[] { new KeyMapping(0, 1, 3) } },
            { "!", new[] { new KeyMapping(0, 2, 3) } },
            { "/", new[] { new KeyMapping(0, 3, 3) } },
            { ".", new[] { new KeyMapping(0, 4, 3) } },
            { ",", new[] { new KeyMapping(0, 5, 3) } },

            { "×", new[] { new KeyMapping(1, 0, 3) } },
            { "<X>", new[] { new KeyMapping(1, 0, 3) } },
            { "(", new[] { new KeyMapping(1, 1, 3) } },
            { ")", new[] { new KeyMapping(1, 2, 3) } },
            { ":", new[] { new KeyMapping(1, 3, 3) } },
            { ";", new[] { new KeyMapping(1, 4, 3) } },
            { "[", new[] { new KeyMapping(1, 5, 3) } },
            { "]", new[] { new KeyMapping(1, 6, 3) } },
            { "<PK>", new[] { new KeyMapping(1, 7, 3) } },
            { "<MN>", new[] { new KeyMapping(1, 8, 3) } },

            { " ", new[] { new KeyMapping(0, 8, 2), new KeyMapping(0, 6, 3), new KeyMapping(0, 7, 3), new KeyMapping(0, 8, 3), new KeyMapping(1, 8, 2) } }
        };
    }
}
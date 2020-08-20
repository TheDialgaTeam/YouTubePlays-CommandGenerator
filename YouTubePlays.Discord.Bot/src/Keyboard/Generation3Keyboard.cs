using System.Collections.Generic;
using YouTubePlays.Discord.Bot.Keyboard.Options;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class Generation3Keyboard : Keyboard
    {
        public override string Name { get; } = "Generation 3 (FireRed, LeafGreen, Ruby, Sapphire and Emerald)";

        public override string ShortKey { get; } = "3";

        protected override KeyboardOptions KeyboardOptions { get; } = new KeyboardOptions
        {
            KeyMapSizes = new[] { (8, 3), (8, 3), (6, 3) },
            PreExecuteCommand = "st,rt",
            StartingPosition = (0, 3),
            ModeSwitchDelay = 4
        };

        protected override TouchOptions TouchOptions { get; } = new TouchOptions
        {
            TouchAvailable = false
        };

        protected override int NameLength { get; } = 10;

        protected override Dictionary<string, KeyMapping[]> CharMappings { get; } = new Dictionary<string, KeyMapping[]>
        {
            { "A", new[] { new KeyMapping(0, 0, 0) } },
            { "B", new[] { new KeyMapping(0, 1, 0) } },
            { "C", new[] { new KeyMapping(0, 2, 0) } },
            { "D", new[] { new KeyMapping(0, 3, 0) } },
            { "E", new[] { new KeyMapping(0, 4, 0) } },
            { "F", new[] { new KeyMapping(0, 5, 0) } },
            { "G", new[] { new KeyMapping(0, 0, 1) } },
            { "H", new[] { new KeyMapping(0, 1, 1) } },
            { "I", new[] { new KeyMapping(0, 2, 1) } },
            { "J", new[] { new KeyMapping(0, 3, 1) } },
            { "K", new[] { new KeyMapping(0, 4, 1) } },
            { "L", new[] { new KeyMapping(0, 5, 1) } },
            { "M", new[] { new KeyMapping(0, 0, 2) } },
            { "N", new[] { new KeyMapping(0, 1, 2) } },
            { "O", new[] { new KeyMapping(0, 2, 2) } },
            { "P", new[] { new KeyMapping(0, 3, 2) } },
            { "Q", new[] { new KeyMapping(0, 4, 2) } },
            { "R", new[] { new KeyMapping(0, 5, 2) } },
            { "S", new[] { new KeyMapping(0, 6, 2) } },
            { "T", new[] { new KeyMapping(0, 0, 3) } },
            { "U", new[] { new KeyMapping(0, 1, 3) } },
            { "V", new[] { new KeyMapping(0, 2, 3) } },
            { "W", new[] { new KeyMapping(0, 3, 3) } },
            { "X", new[] { new KeyMapping(0, 4, 3) } },
            { "Y", new[] { new KeyMapping(0, 5, 3) } },
            { "Z", new[] { new KeyMapping(0, 6, 3) } },

            { "a", new[] { new KeyMapping(1, 0, 0) } },
            { "b", new[] { new KeyMapping(1, 1, 0) } },
            { "c", new[] { new KeyMapping(1, 2, 0) } },
            { "d", new[] { new KeyMapping(1, 3, 0) } },
            { "e", new[] { new KeyMapping(1, 4, 0) } },
            { "f", new[] { new KeyMapping(1, 5, 0) } },
            { "g", new[] { new KeyMapping(1, 0, 1) } },
            { "h", new[] { new KeyMapping(1, 1, 1) } },
            { "i", new[] { new KeyMapping(1, 2, 1) } },
            { "j", new[] { new KeyMapping(1, 3, 1) } },
            { "k", new[] { new KeyMapping(1, 4, 1) } },
            { "l", new[] { new KeyMapping(1, 5, 1) } },
            { "m", new[] { new KeyMapping(1, 0, 2) } },
            { "n", new[] { new KeyMapping(1, 1, 2) } },
            { "o", new[] { new KeyMapping(1, 2, 2) } },
            { "p", new[] { new KeyMapping(1, 3, 2) } },
            { "q", new[] { new KeyMapping(1, 4, 2) } },
            { "r", new[] { new KeyMapping(1, 5, 2) } },
            { "s", new[] { new KeyMapping(1, 6, 2) } },
            { "t", new[] { new KeyMapping(1, 0, 3) } },
            { "u", new[] { new KeyMapping(1, 1, 3) } },
            { "v", new[] { new KeyMapping(1, 2, 3) } },
            { "w", new[] { new KeyMapping(1, 3, 3) } },
            { "x", new[] { new KeyMapping(1, 4, 3) } },
            { "y", new[] { new KeyMapping(1, 5, 3) } },
            { "z", new[] { new KeyMapping(1, 6, 3) } },

            { "0", new[] { new KeyMapping(2, 0, 0) } },
            { "1", new[] { new KeyMapping(2, 1, 0) } },
            { "2", new[] { new KeyMapping(2, 2, 0) } },
            { "3", new[] { new KeyMapping(2, 3, 0) } },
            { "4", new[] { new KeyMapping(2, 4, 0) } },
            { "5", new[] { new KeyMapping(2, 0, 1) } },
            { "6", new[] { new KeyMapping(2, 1, 1) } },
            { "7", new[] { new KeyMapping(2, 2, 1) } },
            { "8", new[] { new KeyMapping(2, 3, 1) } },
            { "9", new[] { new KeyMapping(2, 4, 1) } },

            { "!", new[] { new KeyMapping(2, 0, 2) } },
            { "?", new[] { new KeyMapping(2, 1, 2) } },
            { "♂", new[] { new KeyMapping(2, 2, 2) } },
            { "<MALE>", new[] { new KeyMapping(2, 2, 2) } },
            { "♀", new[] { new KeyMapping(2, 3, 2) } },
            { "<FEMALE>", new[] { new KeyMapping(2, 3, 2) } },
            { "/", new[] { new KeyMapping(2, 4, 2) } },
            { "-", new[] { new KeyMapping(2, 5, 2) } },
            { "…", new[] { new KeyMapping(2, 0, 3) } },
            { "<...>", new[] { new KeyMapping(2, 0, 3) } },
            { "“", new[] { new KeyMapping(2, 1, 3) } },
            { "<OPENDOUBLEQUOTE>", new[] { new KeyMapping(2, 1, 3) } },
            { "”", new[] { new KeyMapping(2, 2, 3) } },
            { "<CLOSEDOUBLEQUOTE>", new[] { new KeyMapping(2, 2, 3) } },
            { "‘", new[] { new KeyMapping(2, 3, 3) } },
            { "<OPENQUOTE>", new[] { new KeyMapping(2, 3, 3) } },
            { "’", new[] { new KeyMapping(2, 4, 3) } },
            { "<CLOSEQUOTE>", new[] { new KeyMapping(2, 4, 3) } },

            {
                " ", new[]
                {
                    new KeyMapping(0, 6, 0), new KeyMapping(0, 6, 1), new KeyMapping(0, 7, 2), new KeyMapping(0, 7, 3),
                    new KeyMapping(1, 6, 0), new KeyMapping(1, 6, 1), new KeyMapping(1, 7, 2), new KeyMapping(1, 7, 3),
                    new KeyMapping(2, 5, 0), new KeyMapping(2, 5, 1), new KeyMapping(2, 5, 3)
                }
            }
        };
    }
}
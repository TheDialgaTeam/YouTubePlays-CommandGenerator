using System.Collections.Generic;
using YouTubePlays.Discord.Bot.Keyboard.Options;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class Generation4Keyboard : Keyboard
    {
        public override string Name { get; } = "Generation 4 (Diamond, Pearl, Platinum, HeartGold and SoulSilver)";

        public override string ShortKey { get; } = "4";

        protected override KeyboardOptions KeyboardOptions { get; } = new KeyboardOptions
        {
            KeyMapSizes = new[] { (12, 5), (12, 5), (12, 5) },
            PreExecuteCommand = "st,d",
            StartingPosition = (12, 0),
            ModeSwitchDelay = 4,
            WarpYPosition = false
        };

        protected override TouchOptions TouchOptions { get; } = new TouchOptions
        {
            TouchAvailable = true,
            PreExecuteCommand = "t:16:37",
            ModeSwitchDelay = 4,
            ModeSwitchButton = new[] { (16, 37), (28, 37), (41, 37) }
        };

        protected override int NameLength { get; } = 10;

        protected override Dictionary<string, KeyMapping[]> CharMappings { get; } = new Dictionary<string, KeyMapping[]>
        {
            { "A", new[] { new KeyMapping(0, 0, 0, 13, 51) } },
            { "B", new[] { new KeyMapping(0, 1, 0, 20, 51) } },
            { "C", new[] { new KeyMapping(0, 2, 0, 26, 51) } },
            { "D", new[] { new KeyMapping(0, 3, 0, 32, 51) } },
            { "E", new[] { new KeyMapping(0, 4, 0, 38, 51) } },
            { "F", new[] { new KeyMapping(0, 5, 0, 45, 51) } },
            { "G", new[] { new KeyMapping(0, 6, 0, 51, 51) } },
            { "H", new[] { new KeyMapping(0, 7, 0, 57, 51) } },
            { "I", new[] { new KeyMapping(0, 8, 0, 63, 51) } },
            { "J", new[] { new KeyMapping(0, 9, 0, 69, 51) } },

            { "K", new[] { new KeyMapping(0, 0, 1, 13, 61) } },
            { "L", new[] { new KeyMapping(0, 1, 1, 20, 61) } },
            { "M", new[] { new KeyMapping(0, 2, 1, 26, 61) } },
            { "N", new[] { new KeyMapping(0, 3, 1, 32, 61) } },
            { "O", new[] { new KeyMapping(0, 4, 1, 38, 61) } },
            { "P", new[] { new KeyMapping(0, 5, 1, 45, 61) } },
            { "Q", new[] { new KeyMapping(0, 6, 1, 51, 61) } },
            { "R", new[] { new KeyMapping(0, 7, 1, 57, 61) } },
            { "S", new[] { new KeyMapping(0, 8, 1, 63, 61) } },
            { "T", new[] { new KeyMapping(0, 9, 1, 69, 61) } },

            { "U", new[] { new KeyMapping(0, 0, 2, 13, 71) } },
            { "V", new[] { new KeyMapping(0, 1, 2, 20, 71) } },
            { "W", new[] { new KeyMapping(0, 2, 2, 26, 71) } },
            { "X", new[] { new KeyMapping(0, 3, 2, 32, 71) } },
            { "Y", new[] { new KeyMapping(0, 4, 2, 38, 71) } },
            { "Z", new[] { new KeyMapping(0, 5, 2, 45, 71) } },

            { "a", new[] { new KeyMapping(1, 0, 0, 13, 51) } },
            { "b", new[] { new KeyMapping(1, 1, 0, 20, 51) } },
            { "c", new[] { new KeyMapping(1, 2, 0, 26, 51) } },
            { "d", new[] { new KeyMapping(1, 3, 0, 32, 51) } },
            { "e", new[] { new KeyMapping(1, 4, 0, 38, 51) } },
            { "f", new[] { new KeyMapping(1, 5, 0, 45, 51) } },
            { "g", new[] { new KeyMapping(1, 6, 0, 51, 51) } },
            { "h", new[] { new KeyMapping(1, 7, 0, 57, 51) } },
            { "i", new[] { new KeyMapping(1, 8, 0, 63, 51) } },
            { "j", new[] { new KeyMapping(1, 9, 0, 69, 51) } },

            { "k", new[] { new KeyMapping(1, 0, 1, 13, 61) } },
            { "l", new[] { new KeyMapping(1, 1, 1, 20, 61) } },
            { "m", new[] { new KeyMapping(1, 2, 1, 26, 61) } },
            { "n", new[] { new KeyMapping(1, 3, 1, 32, 61) } },
            { "o", new[] { new KeyMapping(1, 4, 1, 38, 61) } },
            { "p", new[] { new KeyMapping(1, 5, 1, 45, 61) } },
            { "q", new[] { new KeyMapping(1, 6, 1, 51, 61) } },
            { "r", new[] { new KeyMapping(1, 7, 1, 57, 61) } },
            { "s", new[] { new KeyMapping(1, 8, 1, 63, 61) } },
            { "t", new[] { new KeyMapping(1, 9, 1, 69, 61) } },

            { "u", new[] { new KeyMapping(1, 0, 2, 13, 71) } },
            { "v", new[] { new KeyMapping(1, 1, 2, 20, 71) } },
            { "w", new[] { new KeyMapping(1, 2, 2, 26, 71) } },
            { "x", new[] { new KeyMapping(1, 3, 2, 32, 71) } },
            { "y", new[] { new KeyMapping(1, 4, 2, 38, 71) } },
            { "z", new[] { new KeyMapping(1, 5, 2, 45, 71) } },

            { "0", new[] { new KeyMapping(0, 0, 4, 13, 91), new KeyMapping(1, 0, 4, 13, 91) } },
            { "1", new[] { new KeyMapping(0, 1, 4, 20, 91), new KeyMapping(1, 1, 4, 20, 91) } },
            { "2", new[] { new KeyMapping(0, 2, 4, 26, 91), new KeyMapping(1, 2, 4, 26, 91) } },
            { "3", new[] { new KeyMapping(0, 3, 4, 32, 91), new KeyMapping(1, 3, 4, 32, 91) } },
            { "4", new[] { new KeyMapping(0, 4, 4, 38, 91), new KeyMapping(1, 4, 4, 38, 91) } },
            { "5", new[] { new KeyMapping(0, 5, 4, 45, 91), new KeyMapping(1, 5, 4, 45, 91) } },
            { "6", new[] { new KeyMapping(0, 6, 4, 51, 91), new KeyMapping(1, 6, 4, 51, 91) } },
            { "7", new[] { new KeyMapping(0, 7, 4, 57, 91), new KeyMapping(1, 7, 4, 57, 91) } },
            { "8", new[] { new KeyMapping(0, 8, 4, 63, 91), new KeyMapping(1, 8, 4, 63, 91) } },
            { "9", new[] { new KeyMapping(0, 9, 4, 69, 91), new KeyMapping(1, 9, 4, 69, 91) } },

            { ",", new[] { new KeyMapping(0, 11, 0, 82, 51), new KeyMapping(1, 11, 0, 82, 51), new KeyMapping(2, 0, 0, 13, 51) } },
            { ".", new[] { new KeyMapping(0, 12, 0, 88, 51), new KeyMapping(1, 12, 0, 88, 51), new KeyMapping(2, 1, 0, 20, 51) } },
            { "’", new[] { new KeyMapping(0, 11, 1, 82, 61), new KeyMapping(1, 11, 1, 82, 61), new KeyMapping(2, 3, 1, 32, 61) } },
            { "<CLOSEQUOTE>", new[] { new KeyMapping(0, 11, 1, 82, 61), new KeyMapping(1, 11, 1, 82, 61), new KeyMapping(2, 3, 1, 32, 61) } },
            { "-", new[] { new KeyMapping(0, 12, 1, 88, 61), new KeyMapping(1, 12, 1, 88, 61), new KeyMapping(2, 7, 2, 57, 71) } },
            { "♂", new[] { new KeyMapping(0, 11, 2, 82, 71), new KeyMapping(1, 11, 2, 82, 71), new KeyMapping(2, 9, 0, 69, 51) } },
            { "<MALE>", new[] { new KeyMapping(0, 11, 2, 82, 71), new KeyMapping(1, 11, 2, 82, 71), new KeyMapping(2, 9, 0, 69, 51) } },
            { "♀", new[] { new KeyMapping(0, 12, 2, 88, 71), new KeyMapping(1, 12, 2, 88, 71), new KeyMapping(2, 10, 0, 76, 51) } },
            { "<FEMALE>", new[] { new KeyMapping(0, 12, 2, 88, 71), new KeyMapping(1, 12, 2, 88, 71), new KeyMapping(2, 10, 0, 76, 51) } },

            { ":", new[] { new KeyMapping(2, 2, 0, 26, 51) } },
            { ";", new[] { new KeyMapping(2, 3, 0, 32, 51) } },
            { "!", new[] { new KeyMapping(2, 4, 0, 38, 51) } },
            { "?", new[] { new KeyMapping(2, 5, 0, 45, 51) } },
            { "“", new[] { new KeyMapping(2, 0, 1, 13, 61) } },
            { "<OPENDOUBLEQUOTE>", new[] { new KeyMapping(2, 0, 1, 13, 61) } },
            { "”", new[] { new KeyMapping(2, 1, 1, 20, 61) } },
            { "<CLOSEDOUBLEQUOTE>", new[] { new KeyMapping(2, 1, 1, 20, 61) } },
            { "‘", new[] { new KeyMapping(2, 2, 1, 26, 61) } },
            { "<OPENQUOTE>", new[] { new KeyMapping(2, 2, 1, 26, 61) } },
            { "(", new[] { new KeyMapping(2, 4, 1, 38, 61) } },
            { ")", new[] { new KeyMapping(2, 5, 1, 45, 61) } },
            { "…", new[] { new KeyMapping(2, 0, 2, 13, 71) } },
            { "<...>", new[] { new KeyMapping(2, 0, 2, 13, 71) } },
            { "•", new[] { new KeyMapping(2, 1, 2, 20, 71) } },
            { "<DOT>", new[] { new KeyMapping(2, 1, 2, 20, 71) } },
            { "~", new[] { new KeyMapping(2, 2, 2, 26, 71) } },
            { "@", new[] { new KeyMapping(2, 3, 2, 32, 71) } },
            { "#", new[] { new KeyMapping(2, 4, 2, 38, 71) } },
            { "%", new[] { new KeyMapping(2, 5, 2, 45, 71) } },
            { "+", new[] { new KeyMapping(2, 6, 2, 51, 71) } },
            { "*", new[] { new KeyMapping(2, 8, 2, 63, 71) } },
            { "/", new[] { new KeyMapping(2, 9, 2, 69, 71) } },
            { "=", new[] { new KeyMapping(2, 10, 2, 76, 71) } },

            { "<DOUBLECIRCLE>", new[] { new KeyMapping(2, 0, 3, 13, 81) } },
            { "<CIRCLE>", new[] { new KeyMapping(2, 1, 3, 20, 81) } },
            { "<SQUARE>", new[] { new KeyMapping(2, 2, 3, 26, 81) } },
            { "<TRIANGLE>", new[] { new KeyMapping(2, 3, 3, 32, 81) } },
            { "<EMPTYDIAMOND>", new[] { new KeyMapping(2, 4, 3, 38, 81) } },
            { "<SPADE>", new[] { new KeyMapping(2, 5, 3, 45, 81) } },
            { "<HEART>", new[] { new KeyMapping(2, 6, 3, 51, 81) } },
            { "<DIAMOND>", new[] { new KeyMapping(2, 7, 3, 57, 81) } },
            { "<CLUB>", new[] { new KeyMapping(2, 8, 3, 63, 81) } },
            { "<STAR>", new[] { new KeyMapping(2, 9, 3, 69, 81) } },
            { "<NOTE>", new[] { new KeyMapping(2, 10, 3, 76, 81) } },

            { "<SUN>", new[] { new KeyMapping(2, 0, 4, 13, 91) } },
            { "<CLOUD>", new[] { new KeyMapping(2, 1, 4, 20, 91) } },
            { "<UMBRELLA>", new[] { new KeyMapping(2, 2, 4, 26, 91) } },
            { "<SNOWMAN>", new[] { new KeyMapping(2, 3, 4, 32, 91) } },
            { "<:)>", new[] { new KeyMapping(2, 4, 4, 38, 91) } },
            { "<:D>", new[] { new KeyMapping(2, 5, 4, 45, 91) } },
            { "<SADFACE>", new[] { new KeyMapping(2, 6, 4, 51, 91) } },
            { "<ANGRYFACE>", new[] { new KeyMapping(2, 7, 4, 57, 91) } },
            { "<SLEEP>", new[] { new KeyMapping(2, 8, 4, 63, 91) } },
            { "<UPARROW>", new[] { new KeyMapping(2, 9, 4, 69, 91) } },
            { "<DOWNARROW>", new[] { new KeyMapping(2, 10, 4, 76, 91) } },

            {
                " ", new[]
                {
                    new KeyMapping(0, 10, 0, 76, 51),
                    new KeyMapping(0, 10, 1, 76, 61),
                    new KeyMapping(0, 6, 2, 51, 71), new KeyMapping(0, 7, 2, 57, 71), new KeyMapping(0, 8, 2, 63, 71), new KeyMapping(0, 9, 2, 69, 71), new KeyMapping(0, 10, 2, 76, 71),
                    new KeyMapping(0, 0, 3, 13, 81), new KeyMapping(0, 1, 3, 20, 81), new KeyMapping(0, 2, 3, 26, 81), new KeyMapping(0, 3, 3, 32, 81), new KeyMapping(0, 4, 3, 38, 81), new KeyMapping(0, 5, 3, 45, 81), new KeyMapping(0, 6, 3, 51, 81), new KeyMapping(0, 7, 3, 57, 81), new KeyMapping(0, 8, 3, 63, 81), new KeyMapping(0, 9, 3, 69, 81), new KeyMapping(0, 10, 3, 76, 81), new KeyMapping(0, 11, 3, 82, 81), new KeyMapping(0, 12, 3, 88, 81),
                    new KeyMapping(0, 10, 4, 76, 91), new KeyMapping(0, 11, 4, 82, 91), new KeyMapping(0, 12, 4, 88, 91),

                    new KeyMapping(1, 10, 0, 76, 51),
                    new KeyMapping(1, 10, 1, 76, 61),
                    new KeyMapping(1, 6, 2, 51, 71), new KeyMapping(1, 7, 2, 57, 71), new KeyMapping(1, 8, 2, 63, 71), new KeyMapping(1, 9, 2, 69, 71), new KeyMapping(1, 10, 2, 76, 71),
                    new KeyMapping(1, 0, 3, 13, 81), new KeyMapping(1, 1, 3, 20, 81), new KeyMapping(1, 2, 3, 26, 81), new KeyMapping(1, 3, 3, 32, 81), new KeyMapping(1, 4, 3, 38, 81), new KeyMapping(1, 5, 3, 45, 81), new KeyMapping(1, 6, 3, 51, 81), new KeyMapping(1, 7, 3, 57, 81), new KeyMapping(1, 8, 3, 63, 81), new KeyMapping(1, 9, 3, 69, 81), new KeyMapping(1, 10, 3, 76, 81), new KeyMapping(1, 11, 3, 82, 81), new KeyMapping(1, 12, 3, 88, 81),
                    new KeyMapping(1, 10, 4, 76, 91), new KeyMapping(1, 11, 4, 82, 91), new KeyMapping(1, 12, 4, 88, 91),

                    new KeyMapping(2, 6, 0, 51, 51), new KeyMapping(2, 7, 0, 57, 51), new KeyMapping(2, 8, 0, 63, 51), new KeyMapping(2, 11, 0, 82, 51), new KeyMapping(2, 12, 0, 88, 51),
                    new KeyMapping(2, 6, 1, 51, 61), new KeyMapping(2, 7, 1, 57, 61), new KeyMapping(2, 8, 1, 63, 61), new KeyMapping(2, 9, 1, 69, 61), new KeyMapping(2, 10, 1, 76, 61), new KeyMapping(2, 11, 1, 82, 61), new KeyMapping(2, 12, 1, 88, 61),
                    new KeyMapping(2, 11, 2, 82, 71), new KeyMapping(2, 12, 2, 88, 71),
                    new KeyMapping(2, 11, 3, 82, 81), new KeyMapping(2, 12, 3, 88, 81),
                    new KeyMapping(2, 11, 4, 82, 91), new KeyMapping(2, 12, 4, 88, 91)
                }
            }
        };
    }
}
using System.Collections.Generic;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class Generation7Keyboard : Keyboard
    {
        public override string Name { get; } = "Generation 7 (Sun, Moon, Ultra Sun and Ultra Moon)";

        public override string ShortKey { get; } = "7";

        public override KeyboardOptions KeyboardOptions { get; } = new KeyboardOptions
        {
            KeyMapSizes = new[] { (12, 5), (12, 5), (12, 5) },
            PreExecuteCommand = "l",
            StartingPosition = (0, 0),
            ModeSwitchCommand = "y",
            PostModeSwitchPosition = new[] { (-1, -1), (-1, -1), (-1, -1) }
        };

        public override TouchOptions TouchOptions { get; } = new TouchOptions
        {
            TouchAvailable = true,
            PreExecuteCommand = "l,t:15:89",
            ModeSwitchButton = new[] { (15, 89), (28, 89), (40, 89) }
        };

        public override int NameLength { get; } = 12;

        public override Dictionary<string, KeyMapping[]> CharMappings { get; } = new Dictionary<string, KeyMapping[]>
        {
            { "A", new[] { new KeyMapping(0, 0, 0, 12, 27) } },
            { "B", new[] { new KeyMapping(0, 1, 0, 19, 27) } },
            { "C", new[] { new KeyMapping(0, 2, 0, 25, 27) } },
            { "D", new[] { new KeyMapping(0, 3, 0, 31, 27) } },
            { "E", new[] { new KeyMapping(0, 4, 0, 37, 27) } },
            { "F", new[] { new KeyMapping(0, 5, 0, 44, 27) } },
            { "G", new[] { new KeyMapping(0, 6, 0, 50, 27) } },
            { "H", new[] { new KeyMapping(0, 7, 0, 56, 27) } },
            { "I", new[] { new KeyMapping(0, 8, 0, 62, 27) } },
            { "J", new[] { new KeyMapping(0, 9, 0, 68, 27) } },

            { "K", new[] { new KeyMapping(0, 0, 1, 12, 39) } },
            { "L", new[] { new KeyMapping(0, 1, 1, 19, 39) } },
            { "M", new[] { new KeyMapping(0, 2, 1, 25, 39) } },
            { "N", new[] { new KeyMapping(0, 3, 1, 31, 39) } },
            { "O", new[] { new KeyMapping(0, 4, 1, 37, 39) } },
            { "P", new[] { new KeyMapping(0, 5, 1, 44, 39) } },
            { "Q", new[] { new KeyMapping(0, 6, 1, 50, 39) } },
            { "R", new[] { new KeyMapping(0, 7, 1, 56, 39) } },
            { "S", new[] { new KeyMapping(0, 8, 1, 62, 39) } },
            { "T", new[] { new KeyMapping(0, 9, 1, 68, 39) } },

            { "U", new[] { new KeyMapping(0, 0, 2, 12, 52) } },
            { "V", new[] { new KeyMapping(0, 1, 2, 19, 52) } },
            { "W", new[] { new KeyMapping(0, 2, 2, 25, 52) } },
            { "X", new[] { new KeyMapping(0, 3, 2, 31, 52) } },
            { "Y", new[] { new KeyMapping(0, 4, 2, 37, 52) } },
            { "Z", new[] { new KeyMapping(0, 5, 2, 44, 52) } },

            { "É", new[] { new KeyMapping(0, 0, 3, 12, 64) } },

            { "a", new[] { new KeyMapping(1, 0, 0, 12, 27) } },
            { "b", new[] { new KeyMapping(1, 1, 0, 19, 27) } },
            { "c", new[] { new KeyMapping(1, 2, 0, 25, 27) } },
            { "d", new[] { new KeyMapping(1, 3, 0, 31, 27) } },
            { "e", new[] { new KeyMapping(1, 4, 0, 37, 27) } },
            { "f", new[] { new KeyMapping(1, 5, 0, 44, 27) } },
            { "g", new[] { new KeyMapping(1, 6, 0, 50, 27) } },
            { "h", new[] { new KeyMapping(1, 7, 0, 56, 27) } },
            { "i", new[] { new KeyMapping(1, 8, 0, 62, 27) } },
            { "j", new[] { new KeyMapping(1, 9, 0, 68, 27) } },

            { "k", new[] { new KeyMapping(1, 0, 1, 12, 39) } },
            { "l", new[] { new KeyMapping(1, 1, 1, 19, 39) } },
            { "m", new[] { new KeyMapping(1, 2, 1, 25, 39) } },
            { "n", new[] { new KeyMapping(1, 3, 1, 31, 39) } },
            { "o", new[] { new KeyMapping(1, 4, 1, 37, 39) } },
            { "p", new[] { new KeyMapping(1, 5, 1, 44, 39) } },
            { "q", new[] { new KeyMapping(1, 6, 1, 50, 39) } },
            { "r", new[] { new KeyMapping(1, 7, 1, 56, 39) } },
            { "s", new[] { new KeyMapping(1, 8, 1, 62, 39) } },
            { "t", new[] { new KeyMapping(1, 9, 1, 68, 39) } },

            { "u", new[] { new KeyMapping(1, 0, 2, 12, 52) } },
            { "v", new[] { new KeyMapping(1, 1, 2, 19, 52) } },
            { "w", new[] { new KeyMapping(1, 2, 2, 25, 52) } },
            { "x", new[] { new KeyMapping(1, 3, 2, 31, 52) } },
            { "y", new[] { new KeyMapping(1, 4, 2, 37, 52) } },
            { "z", new[] { new KeyMapping(1, 5, 2, 44, 52) } },

            { "é", new[] { new KeyMapping(1, 0, 3, 12, 64) } },

            { "0", new[] { new KeyMapping(0, 0, 4, 12, 77), new KeyMapping(1, 0, 4, 12, 77) } },
            { "1", new[] { new KeyMapping(0, 1, 4, 19, 77), new KeyMapping(1, 1, 4, 19, 77) } },
            { "2", new[] { new KeyMapping(0, 2, 4, 25, 77), new KeyMapping(1, 2, 4, 25, 77) } },
            { "3", new[] { new KeyMapping(0, 3, 4, 31, 77), new KeyMapping(1, 3, 4, 31, 77) } },
            { "4", new[] { new KeyMapping(0, 4, 4, 37, 77), new KeyMapping(1, 4, 4, 37, 77) } },
            { "5", new[] { new KeyMapping(0, 5, 4, 44, 77), new KeyMapping(1, 5, 4, 44, 77) } },
            { "6", new[] { new KeyMapping(0, 6, 4, 50, 77), new KeyMapping(1, 6, 4, 50, 77) } },
            { "7", new[] { new KeyMapping(0, 7, 4, 56, 77), new KeyMapping(1, 7, 4, 56, 77) } },
            { "8", new[] { new KeyMapping(0, 8, 4, 62, 77), new KeyMapping(1, 8, 4, 62, 77) } },
            { "9", new[] { new KeyMapping(0, 9, 4, 68, 77), new KeyMapping(1, 9, 4, 68, 77) } },

            { ",", new[] { new KeyMapping(0, 11, 0, 81, 27), new KeyMapping(1, 11, 0, 81, 27), new KeyMapping(2, 0, 0, 12, 27) } },
            { ".", new[] { new KeyMapping(0, 12, 0, 87, 27), new KeyMapping(1, 12, 0, 87, 27), new KeyMapping(2, 1, 0, 19, 27) } },
            { "’", new[] { new KeyMapping(0, 11, 1, 81, 39), new KeyMapping(1, 11, 1, 81, 39), new KeyMapping(2, 3, 1, 31, 39) } },
            { "<CLOSEQUOTE>", new[] { new KeyMapping(0, 11, 1, 81, 39), new KeyMapping(1, 11, 1, 81, 39), new KeyMapping(2, 3, 1, 31, 39) } },
            { "-", new[] { new KeyMapping(0, 12, 1, 87, 39), new KeyMapping(1, 12, 1, 87, 39), new KeyMapping(2, 6, 2, 50, 52) } },
            { "♂", new[] { new KeyMapping(0, 11, 2, 81, 52), new KeyMapping(1, 11, 2, 81, 52), new KeyMapping(2, 9, 0, 68, 27) } },
            { "<MALE>", new[] { new KeyMapping(0, 11, 2, 81, 52), new KeyMapping(1, 11, 2, 81, 52), new KeyMapping(2, 9, 0, 68, 27) } },
            { "♀", new[] { new KeyMapping(0, 12, 2, 87, 52), new KeyMapping(1, 12, 2, 87, 52), new KeyMapping(2, 10, 0, 75, 27) } },
            { "<FEMALE>", new[] { new KeyMapping(0, 12, 2, 87, 52), new KeyMapping(1, 12, 2, 87, 52), new KeyMapping(2, 10, 0, 75, 27) } },
            { "!", new[] { new KeyMapping(0, 11, 3, 81, 64), new KeyMapping(1, 11, 3, 81, 64), new KeyMapping(2, 4, 0, 37, 27) } },
            { "?", new[] { new KeyMapping(0, 12, 3, 87, 64), new KeyMapping(1, 12, 3, 87, 64), new KeyMapping(2, 5, 0, 44, 27) } },

            { ":", new[] { new KeyMapping(2, 2, 0, 25, 27) } },
            { ";", new[] { new KeyMapping(2, 3, 0, 31, 27) } },
            { "“", new[] { new KeyMapping(2, 0, 1, 12, 39) } },
            { "<OPENDOUBLEQUOTE>", new[] { new KeyMapping(2, 0, 1, 12, 39) } },
            { "”", new[] { new KeyMapping(2, 1, 1, 19, 39) } },
            { "<CLOSEDOUBLEQUOTE>", new[] { new KeyMapping(2, 1, 1, 19, 39) } },
            { "‘", new[] { new KeyMapping(2, 2, 1, 25, 39) } },
            { "<OPENQUOTE>", new[] { new KeyMapping(2, 2, 1, 25, 39) } },
            { "(", new[] { new KeyMapping(2, 4, 1, 37, 39) } },
            { ")", new[] { new KeyMapping(2, 5, 1, 44, 39) } },
            { "…", new[] { new KeyMapping(2, 0, 2, 12, 52) } },
            { "<...>", new[] { new KeyMapping(2, 0, 2, 12, 52) } },
            { "•", new[] { new KeyMapping(2, 1, 2, 19, 52) } },
            { "<DOT>", new[] { new KeyMapping(2, 1, 2, 19, 52) } },
            { "~", new[] { new KeyMapping(2, 2, 2, 25, 52) } },
            { "#", new[] { new KeyMapping(2, 3, 2, 31, 52) } },
            { "%", new[] { new KeyMapping(2, 4, 2, 37, 52) } },
            { "+", new[] { new KeyMapping(2, 5, 2, 44, 52) } },
            { "*", new[] { new KeyMapping(2, 7, 2, 56, 52) } },
            { "/", new[] { new KeyMapping(2, 8, 2, 62, 52) } },
            { "=", new[] { new KeyMapping(2, 9, 2, 68, 52) } },

            { "<DOUBLECIRCLE>", new[] { new KeyMapping(2, 0, 3, 12, 64) } },
            { "<CIRCLE>", new[] { new KeyMapping(2, 1, 3, 19, 64) } },
            { "<SQUARE>", new[] { new KeyMapping(2, 2, 3, 25, 64) } },
            { "<TRIANGLE>", new[] { new KeyMapping(2, 3, 3, 31, 64) } },
            { "<EMPTYDIAMOND>", new[] { new KeyMapping(2, 4, 3, 37, 64) } },
            { "<SPADE>", new[] { new KeyMapping(2, 5, 3, 44, 64) } },
            { "<HEART>", new[] { new KeyMapping(2, 6, 3, 50, 64) } },
            { "<DIAMOND>", new[] { new KeyMapping(2, 7, 3, 56, 64) } },
            { "<CLUB>", new[] { new KeyMapping(2, 8, 3, 62, 64) } },
            { "<STAR>", new[] { new KeyMapping(2, 9, 3, 68, 64) } },
            { "<NOTE>", new[] { new KeyMapping(2, 10, 3, 75, 64) } },

            { "<SUN>", new[] { new KeyMapping(2, 0, 4, 12, 77) } },
            { "<CLOUD>", new[] { new KeyMapping(2, 1, 4, 19, 77) } },
            { "<UMBRELLA>", new[] { new KeyMapping(2, 2, 4, 25, 77) } },
            { "<SNOWMAN>", new[] { new KeyMapping(2, 3, 4, 31, 77) } },
            { "<-_->", new[] { new KeyMapping(2, 4, 4, 37, 77) } },
            { "<:)>", new[] { new KeyMapping(2, 5, 4, 44, 77) } },
            { "<SADFACE>", new[] { new KeyMapping(2, 6, 4, 50, 77) } },
            { "<ANGRYFACE>", new[] { new KeyMapping(2, 7, 4, 56, 77) } },
            { "<SLEEP>", new[] { new KeyMapping(2, 8, 4, 62, 77) } },
            { "<UPARROW>", new[] { new KeyMapping(2, 9, 4, 68, 77) } },
            { "<DOWNARROW>", new[] { new KeyMapping(2, 10, 4, 75, 77) } },

            {
                " ", new[]
                {
                    new KeyMapping(0, 10, 0, 75, 27),
                    new KeyMapping(0, 10, 1, 75, 39),
                    new KeyMapping(0, 6, 2, 50, 52), new KeyMapping(0, 7, 2, 56, 52), new KeyMapping(0, 8, 2, 62, 52), new KeyMapping(0, 9, 2, 68, 52), new KeyMapping(0, 10, 2, 75, 52),
                    new KeyMapping(0, 1, 3, 19, 64), new KeyMapping(0, 2, 3, 25, 64), new KeyMapping(0, 3, 3, 31, 64), new KeyMapping(0, 4, 3, 37, 64), new KeyMapping(0, 5, 3, 44, 64), new KeyMapping(0, 6, 3, 50, 64), new KeyMapping(0, 7, 3, 56, 64), new KeyMapping(0, 8, 3, 62, 64), new KeyMapping(0, 9, 3, 68, 64), new KeyMapping(0, 10, 3, 75, 64),
                    new KeyMapping(0, 10, 4, 75, 77), new KeyMapping(0, 11, 4, 81, 77), new KeyMapping(0, 12, 4, 87, 77),

                    new KeyMapping(1, 10, 0, 75, 27),
                    new KeyMapping(1, 10, 1, 75, 39),
                    new KeyMapping(1, 6, 2, 50, 52), new KeyMapping(1, 7, 2, 56, 52), new KeyMapping(1, 8, 2, 62, 52), new KeyMapping(1, 9, 2, 68, 52), new KeyMapping(1, 10, 2, 75, 52),
                    new KeyMapping(1, 1, 3, 19, 64), new KeyMapping(1, 2, 3, 25, 64), new KeyMapping(1, 3, 3, 31, 64), new KeyMapping(1, 4, 3, 37, 64), new KeyMapping(1, 5, 3, 44, 64), new KeyMapping(1, 6, 3, 50, 64), new KeyMapping(1, 7, 3, 56, 64), new KeyMapping(1, 8, 3, 62, 64), new KeyMapping(1, 9, 3, 68, 64), new KeyMapping(1, 10, 3, 75, 64),
                    new KeyMapping(1, 10, 4, 75, 77), new KeyMapping(1, 11, 4, 81, 77), new KeyMapping(1, 12, 4, 87, 77),

                    new KeyMapping(2, 6, 0, 50, 27), new KeyMapping(2, 7, 0, 56, 27), new KeyMapping(2, 8, 0, 62, 27), new KeyMapping(2, 11, 0, 81, 27), new KeyMapping(2, 12, 0, 87, 27),
                    new KeyMapping(2, 6, 1, 50, 39), new KeyMapping(2, 7, 1, 56, 39), new KeyMapping(2, 8, 1, 62, 39), new KeyMapping(2, 9, 1, 68, 39), new KeyMapping(2, 10, 1, 75, 39), new KeyMapping(2, 11, 1, 81, 39), new KeyMapping(2, 12, 1, 87, 39),
                    new KeyMapping(2, 10, 2, 75, 52), new KeyMapping(2, 11, 2, 81, 52), new KeyMapping(2, 12, 2, 87, 52),
                    new KeyMapping(2, 11, 3, 81, 64), new KeyMapping(2, 12, 3, 87, 64),
                    new KeyMapping(2, 11, 4, 81, 77), new KeyMapping(2, 12, 4, 87, 77)
                }
            }
        };
    }
}
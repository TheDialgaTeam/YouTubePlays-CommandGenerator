using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using YouTubePlays.Discord.Bot.EntityFramework.Table;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public class KeyboardCollection
    {
        public IKeyboard[] Keyboards { get; } = {
            new Generation1Keyboard(),
            new Generation2Keyboard(),
            new Generation3Keyboard(),
            new Generation4Keyboard(),
            new Generation5Keyboard(),
            new Generation6Keyboard(),
            new Generation7Keyboard()
        };

        public bool TryGetKeyboard(string key, out IKeyboard keyboard)
        {
            foreach (var currentKeyboard in Keyboards)
            {
                if (!currentKeyboard.ShortKey.Equals(key, StringComparison.OrdinalIgnoreCase)) continue;
                
                keyboard = currentKeyboard;
                return true;
            }

            keyboard = new Generation1Keyboard();
            return false;
        }

        public string[] GetCommands(string name, ChannelSettings channelSettings)
        {
            var result = new List<string>();
            var commandBuilder = new StringBuilder();
            var currentIndex = 0;

            if (!TryGetKeyboard(channelSettings.KeyboardType, out var keyboard)) return new string[0];

            var fullCommand = keyboard.GetChatCommand(name, channelSettings).Split(',');

            foreach (var command in fullCommand)
            {
                if (string.IsNullOrWhiteSpace(command)) continue;

                var match = Regex.Match(command, "([a-z]+)(\\d+)");

                if (match.Success)
                {
                    var actualCommand = match.Groups[1].Value;
                    var amount = Convert.ToInt32(match.Groups[2].Value);

                    while (amount > 0)
                    {
                        var amountToFill = channelSettings.InputLimit - currentIndex;

                        if (amount <= amountToFill)
                        {
                            if (currentIndex != 0) commandBuilder.Append(",");
                            commandBuilder.Append(actualCommand);
                            if (amount > 1) commandBuilder.Append(amount);
                            currentIndex += amount;
                            break;
                        }

                        if (currentIndex != 0) commandBuilder.Append(",");
                        commandBuilder.Append(actualCommand);
                        if (amountToFill > 1) commandBuilder.Append(amountToFill);

                        result.Add(commandBuilder.ToString());
                        commandBuilder.Clear();

                        amount -= amountToFill;
                        currentIndex = 0;
                    }
                }
                else
                {
                    if (currentIndex != 0) commandBuilder.Append(",");
                    commandBuilder.Append(command);
                    currentIndex++;
                }

                if (currentIndex != channelSettings.InputLimit) continue;

                result.Add(commandBuilder.ToString());
                commandBuilder.Clear();
                currentIndex = 0;
            }

            if (commandBuilder.Length > 0) result.Add(commandBuilder.ToString());

            return result.ToArray();
        }
    }
}
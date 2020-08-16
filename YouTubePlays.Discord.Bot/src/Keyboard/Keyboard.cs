using System;
using System.Collections.Generic;
using System.Text;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public abstract class Keyboard : IKeyboard
    {
        public abstract string Name { get; }

        public abstract string ShortKey { get; }

        public abstract KeyboardOptions KeyboardOptions { get; }

        public abstract TouchOptions TouchOptions { get; }

        public abstract int NameLength { get; }

        public abstract Dictionary<string, KeyMapping[]> CharMappings { get; }

        public string GetChatCommand(string name, ChatBot chatBot)
        {
            return TouchOptions.TouchAvailable && chatBot.TouchAvailable ? GetTouchCommand(name, chatBot) : GetKeyboardCommand(name);
        }

        private string GetKeyboardCommand(string name)
        {
            var nameSpan = name.AsSpan();
            var nameLength = nameSpan.Length;

            var keyboardCommand = new StringBuilder(KeyboardOptions.PreExecuteCommand);

            var currentIndex = 0;
            var currentPosition = KeyboardOptions.StartingPosition;
            var currentMode = 0;
            var currentNameLength = 0;

            while (currentIndex < nameLength)
            {
                if (currentNameLength + 1 > NameLength) throw new Exception("Name length exceeded.");

                var character = nameSpan[currentIndex];

                if (character != '<')
                {
                    if (CharMappings.TryGetValue(character.ToString(), out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestKeyboardCharMap(currentMode, currentPosition, keyMappings);
                        currentPosition = (targetKeyMapping.X, targetKeyMapping.Y);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex++;
                }
                else
                {
                    // There are escape char.
                    var escapeSpan = nameSpan.Slice(currentIndex);
                    var charmapSpan = escapeSpan.Slice(0, escapeSpan.IndexOf('>') + 1);

                    if (CharMappings.TryGetValue(charmapSpan.ToString(), out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestKeyboardCharMap(currentMode, currentPosition, keyMappings);
                        currentPosition = (targetKeyMapping.X, targetKeyMapping.Y);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex += charmapSpan.Length;
                }

                currentNameLength++;
            }

            keyboardCommand.Append(",st,a");

            return keyboardCommand.ToString();
        }

        private (KeyMapping keyMapping, string command) GetNearestKeyboardCharMap(int currentMode, (int x, int y) currentPosition, KeyMapping[] keyMappings)
        {
            var getDistanceAndCommand = new Func<int, (int x, int y), KeyMapping, KeyboardOptions, (int totalDistance, string command)>((mode, position, keyMapping, keyboardOptions) =>
            {
                var totalDistance = 0;
                var (x, y) = position;
                var (keyMapSizeX, keyMapSizeY) = keyboardOptions.KeyMapSizes[keyMapping.Mode];
                var command = new StringBuilder();

                if (mode != keyMapping.Mode)
                {
                    totalDistance = Math.Abs(keyMapping.Mode - mode);

                    var modeSwitchDelay = keyboardOptions.ModeSwitchDelay;

                    if (modeSwitchDelay == 0)
                    {
                        command.Append(",");
                        command.Append(keyboardOptions.ModeSwitchCommand);
                        if (totalDistance > 1) command.Append(totalDistance);
                    }
                    else
                    {
                        for (var i = 0; i < totalDistance; i++)
                        {
                            command.Append(",");
                            command.Append(keyboardOptions.ModeSwitchCommand);
                            command.Append(",p");
                            command.Append(modeSwitchDelay);
                        }
                    }

                    // Location adjustment because the keymap size is different.
                    if (x > keyMapSizeX - 1) x = keyMapSizeX - 1;
                    if (y > keyMapSizeY - 1) y = keyMapSizeY - 1;
                }

                if (keyMapping.X > x)
                {
                    var distanceRight = keyMapping.X - x;
                    var distanceLeft = x + 1 + (keyMapSizeX - keyMapping.X);

                    if (distanceRight > distanceLeft)
                    {
                        command.Append(",lt");
                        if (distanceLeft > 1) command.Append(distanceLeft);
                    }
                    else
                    {
                        command.Append(",rt");
                        if (distanceRight > 1) command.Append(distanceRight);
                    }

                    totalDistance += Math.Min(distanceRight, distanceLeft);
                }
                else if (keyMapping.X < x)
                {
                    var distanceLeft = x - keyMapping.X;
                    var distanceRight = keyMapSizeX - x + 1 + keyMapping.X;

                    if (distanceLeft > distanceRight)
                    {
                        command.Append(",rt");
                        if (distanceRight > 1) command.Append(distanceRight);
                    }
                    else
                    {
                        command.Append(",lt");
                        if (distanceLeft > 1) command.Append(distanceLeft);
                    }

                    totalDistance += Math.Min(distanceLeft, distanceRight);
                }

                if (keyMapping.Y > y)
                {
                    var distanceDown = keyMapping.Y - y;
                    var distanceUp = y + 1 + (keyMapSizeY - keyMapping.Y);

                    if (distanceDown > distanceUp)
                    {
                        command.Append(",u");
                        if (distanceUp > 1) command.Append(distanceUp);
                    }
                    else
                    {
                        command.Append(",d");
                        if (distanceDown > 1) command.Append(distanceDown);
                    }

                    totalDistance += Math.Min(distanceDown, distanceUp);
                }
                else if (keyMapping.Y < y)
                {
                    var distanceUp = y - keyMapping.Y;
                    var distanceDown = keyMapSizeY - y + 1 + keyMapping.Y;

                    if (distanceUp > distanceDown)
                    {
                        command.Append(",d");
                        if (distanceDown > 1) command.Append(distanceDown);
                    }
                    else
                    {
                        command.Append(",u");
                        if (distanceUp > 1) command.Append(distanceUp);
                    }

                    totalDistance += Math.Min(distanceUp, distanceDown);
                }

                command.Append(",a");

                return (totalDistance, command.ToString());
            });

            var bestMapping = keyMappings[0];
            var distanceAndCommand = getDistanceAndCommand(currentMode, currentPosition, bestMapping, KeyboardOptions);

            for (var i = 1; i < keyMappings.Length; i++)
            {
                var targetDistanceAndCommand = getDistanceAndCommand(currentMode, currentPosition, keyMappings[i], KeyboardOptions);

                if (targetDistanceAndCommand.totalDistance >= distanceAndCommand.totalDistance) continue;

                bestMapping = keyMappings[i];
                distanceAndCommand = targetDistanceAndCommand;
            }

            return (bestMapping, distanceAndCommand.command);
        }

        private string GetTouchCommand(string name, ChatBot chatBot)
        {
            var nameSpan = name.AsSpan();
            var nameLength = nameSpan.Length;

            var keyboardCommand = new StringBuilder(TouchOptions.PreExecuteCommand);

            var currentIndex = 0;
            var currentMode = 0;
            var currentNameLength = 0;

            while (currentIndex < nameLength)
            {
                if (currentNameLength + 1 > NameLength) throw new Exception("Name length exceeded.");

                var character = nameSpan[currentIndex];

                if (character != '<')
                {
                    if (CharMappings.TryGetValue(character.ToString(), out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestTouchCharMap(currentMode, keyMappings, chatBot);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex++;
                }
                else
                {
                    // There are escape char.
                    var escapeSpan = nameSpan.Slice(currentIndex);
                    var charmapSpan = escapeSpan.Slice(0, escapeSpan.IndexOf('>') + 1);

                    if (CharMappings.TryGetValue(charmapSpan.ToString(), out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestTouchCharMap(currentMode, keyMappings, chatBot);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex += charmapSpan.Length;
                }
            }

            keyboardCommand.Append(",st,a");

            return keyboardCommand.ToString();
        }

        private (KeyMapping keyMapping, string command) GetNearestTouchCharMap(int currentMode, KeyMapping[] keyMappings, ChatBot chatbot)
        {
            var getDistanceAndCommand = new Func<int, KeyMapping, TouchOptions, ChatBot, (int totalDistance, string command)>((mode, keyMapping, touchOptions, bot) =>
            {
                var totalDistance = 1;
                var command = new StringBuilder();

                if (mode != keyMapping.Mode)
                {
                    totalDistance++;

                    var (x, y) = touchOptions.ModeSwitchButton[keyMapping.Mode];

                    command.Append(",t:");
                    command.Append(x + bot.TouchXOffset);
                    command.Append(":");
                    command.Append(y + bot.TouchYOffset);
                }

                command.Append(",t:");
                command.Append(keyMapping.TouchX + bot.TouchXOffset);
                command.Append(":");
                command.Append(keyMapping.TouchY + bot.TouchYOffset);

                return (totalDistance, command.ToString());
            });

            var bestMapping = keyMappings[0];
            var distanceAndCommand = getDistanceAndCommand(currentMode, bestMapping, TouchOptions, chatbot);

            for (var i = 1; i < keyMappings.Length; i++)
            {
                var targetDistanceAndCommand = getDistanceAndCommand(currentMode, keyMappings[i], TouchOptions, chatbot);

                if (targetDistanceAndCommand.totalDistance >= distanceAndCommand.totalDistance) continue;

                bestMapping = keyMappings[i];
                distanceAndCommand = targetDistanceAndCommand;
            }

            return (bestMapping, distanceAndCommand.command);
        }
    }
}
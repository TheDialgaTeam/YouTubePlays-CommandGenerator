using System;
using System.Collections.Generic;
using System.Text;
using YouTubePlays.Discord.Bot.EntityFramework.Table;
using YouTubePlays.Discord.Bot.Keyboard.Options;

namespace YouTubePlays.Discord.Bot.Keyboard
{
    public abstract class Keyboard : IKeyboard
    {
        public abstract string Name { get; }

        public abstract string ShortKey { get; }

        protected abstract KeyboardOptions KeyboardOptions { get; }

        protected abstract TouchOptions TouchOptions { get; }

        protected abstract int NameLength { get; }

        protected abstract Dictionary<string, KeyMapping[]> CharMappings { get; }

        public string GetChatCommand(string name, ChannelSettings channelSettings)
        {
            return TouchOptions.TouchAvailable && channelSettings.TouchAvailable ? GetTouchCommand(name, channelSettings) : GetKeyboardCommand(name);
        }

        private string GetKeyboardCommand(string name)
        {
            var nameLength = name.Length;

            var keyboardCommand = new StringBuilder(KeyboardOptions.PreExecuteCommand);

            var currentIndex = 0;
            var currentPosition = KeyboardOptions.StartingPosition;
            var currentMode = 0;
            var currentNameLength = 0;

            while (currentIndex < nameLength)
            {
                if (currentNameLength + 1 > NameLength) throw new Exception("Name length exceeded.");

                var character = name[currentIndex];

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
                    var escapeChar = name.Substring(currentIndex, name.IndexOf('>') + 1);

                    if (CharMappings.TryGetValue(escapeChar, out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestKeyboardCharMap(currentMode, currentPosition, keyMappings);
                        currentPosition = (targetKeyMapping.X, targetKeyMapping.Y);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex += escapeChar.Length;
                }

                currentNameLength++;
            }

            if (string.IsNullOrWhiteSpace(KeyboardOptions.PostExecuteCommand)) return keyboardCommand.ToString();

            keyboardCommand.Append(",");
            keyboardCommand.AppendLine(KeyboardOptions.PostExecuteCommand);

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
                    if (keyMapping.Mode > mode)
                    {
                        totalDistance = keyMapping.Mode - mode;
                    }
                    else
                    {
                        totalDistance = keyboardOptions.KeyMapSizes.Length - mode + keyMapping.Mode;
                    }

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

                    if (!string.IsNullOrWhiteSpace(keyboardOptions.PostModeSwitchCommand))
                    {
                        command.Append(",");
                        command.Append(keyboardOptions.PostModeSwitchCommand);

                        var (postModeSwitchPositionX, postModeSwitchPositionY) = keyboardOptions.PostModeSwitchPosition[keyMapping.Mode];

                        if (postModeSwitchPositionX != -1 && postModeSwitchPositionY != -1)
                        {
                            x = postModeSwitchPositionX;
                            y = postModeSwitchPositionY;
                        }
                    }

                    // Location adjustment because the keymap size is different.
                    if (x > keyMapSizeX - 1) x = keyMapSizeX - 1;
                    if (y > keyMapSizeY - 1) y = keyMapSizeY - 1;
                }

                if (keyMapping.X > x)
                {
                    if (keyboardOptions.WarpXPosition)
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
                    else
                    {
                        var distanceRight = keyMapping.X - x;

                        command.Append(",rt");
                        if (distanceRight > 1) command.Append(distanceRight);

                        totalDistance += distanceRight;
                    }
                }
                else if (keyMapping.X < x)
                {
                    if (keyboardOptions.WarpXPosition)
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
                    else
                    {
                        var distanceLeft = x - keyMapping.X;

                        command.Append(",lt");
                        if (distanceLeft > 1) command.Append(distanceLeft);

                        totalDistance += distanceLeft;
                    }
                }

                if (keyMapping.Y > y)
                {
                    if (keyboardOptions.WarpYPosition)
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
                    else
                    {
                        var distanceDown = keyMapping.Y - y;

                        command.Append(",d");
                        if (distanceDown > 1) command.Append(distanceDown);

                        totalDistance += distanceDown;
                    }
                }
                else if (keyMapping.Y < y)
                {
                    if (keyboardOptions.WarpYPosition)
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
                    else
                    {
                        var distanceUp = y - keyMapping.Y;

                        command.Append(",u");
                        if (distanceUp > 1) command.Append(distanceUp);

                        totalDistance += distanceUp;
                    }
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

        private string GetTouchCommand(string name, ChannelSettings channelSettings)
        {
            var nameLength = name.Length;

            var keyboardCommand = new StringBuilder(TouchOptions.PreExecuteCommand);

            var currentIndex = 0;
            var currentMode = 0;
            var currentNameLength = 0;

            while (currentIndex < nameLength)
            {
                if (currentNameLength + 1 > NameLength) throw new Exception("Name length exceeded.");

                var character = name[currentIndex];

                if (character != '<')
                {
                    if (CharMappings.TryGetValue(character.ToString(), out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestTouchCharMap(currentMode, keyMappings, channelSettings);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex++;
                }
                else
                {
                    // There are escape char.
                    var escapeChar = name.Substring(currentIndex, name.IndexOf('>') + 1);

                    if (CharMappings.TryGetValue(escapeChar, out var keyMappings))
                    {
                        var (targetKeyMapping, command) = GetNearestTouchCharMap(currentMode, keyMappings, channelSettings);
                        currentMode = targetKeyMapping.Mode;

                        keyboardCommand.Append(command);
                    }

                    currentIndex += escapeChar.Length;
                }

                currentNameLength++;
            }

            if (string.IsNullOrWhiteSpace(TouchOptions.PostExecuteCommand)) return keyboardCommand.ToString();

            keyboardCommand.Append(",");
            keyboardCommand.AppendLine(TouchOptions.PostExecuteCommand);

            return keyboardCommand.ToString();
        }

        private (KeyMapping keyMapping, string command) GetNearestTouchCharMap(int currentMode, KeyMapping[] keyMappings, ChannelSettings channelSettings)
        {
            var getDistanceAndCommand = new Func<int, KeyMapping, TouchOptions, ChannelSettings, (int totalDistance, string command)>((mode, keyMapping, touchOptions, settings) =>
            {
                var totalDistance = 1;
                var command = new StringBuilder();

                if (mode != keyMapping.Mode)
                {
                    totalDistance++;

                    var (x, y) = touchOptions.ModeSwitchButton[keyMapping.Mode];

                    command.Append(",t:");
                    command.Append(x + settings.TouchXOffset);
                    command.Append(":");
                    command.Append(y + settings.TouchYOffset);

                    if (touchOptions.ModeSwitchDelay > 0)
                    {
                        command.Append(",p");
                        command.Append(touchOptions.ModeSwitchDelay);
                    }
                }

                command.Append(",t:");
                command.Append(keyMapping.TouchX + settings.TouchXOffset);
                command.Append(":");
                command.Append(keyMapping.TouchY + settings.TouchYOffset);

                return (totalDistance, command.ToString());
            });

            var bestMapping = keyMappings[0];
            var distanceAndCommand = getDistanceAndCommand(currentMode, bestMapping, TouchOptions, channelSettings);

            for (var i = 1; i < keyMappings.Length; i++)
            {
                var targetDistanceAndCommand = getDistanceAndCommand(currentMode, keyMappings[i], TouchOptions, channelSettings);

                if (targetDistanceAndCommand.totalDistance >= distanceAndCommand.totalDistance) continue;

                bestMapping = keyMappings[i];
                distanceAndCommand = targetDistanceAndCommand;
            }

            return (bestMapping, distanceAndCommand.command);
        }
    }
}
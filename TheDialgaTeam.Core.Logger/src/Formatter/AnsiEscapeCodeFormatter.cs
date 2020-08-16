using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Serilog.Parsing;

namespace TheDialgaTeam.Core.Logger.Formatter
{
    public static class AnsiEscapeCodeFormatter
    {
        private const int StandardOutputHandleId = -11;
        private const uint EnableVirtualTerminalProcessingMode = 4;
        private const long InvalidHandleValue = -1;

        private const string AnsiEscapeRegex = "((?:\\u001b\\[[0-9:;<=>?]*[\\s!\"#$%&'()*+,\\-./]*[@A-Z[\\]^_`a-z{|}~])|(?:\\u001b[@A-Z\\[\\]^_]))";
        private static readonly bool IsAnsiEscapeCodeSupported;

        private static readonly char[] PadChars = new string(' ', 1024).ToCharArray();

        private static readonly ConsoleColor DefaultForegroundColor = Console.ForegroundColor;
        private static readonly ConsoleColor DefaultBackgroundColor = Console.BackgroundColor;

        static AnsiEscapeCodeFormatter()
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                IsAnsiEscapeCodeSupported = true;
            }
            else
            {
                var stdout = GetStdHandle(StandardOutputHandleId);

                if (stdout != (IntPtr) InvalidHandleValue && GetConsoleMode(stdout, out var mode))
                {
                    IsAnsiEscapeCodeSupported = SetConsoleMode(stdout, mode | EnableVirtualTerminalProcessingMode);
                }
                else
                {
                    IsAnsiEscapeCodeSupported = false;
                }
            }
        }

        public static void Format(TextWriter textWriter, string text, PropertyToken propertyToken = null)
        {
            var isCompatibleAnsiOutput = textWriter == Console.Out || textWriter == Console.Error;

            if (isCompatibleAnsiOutput)
            {
                if (IsAnsiEscapeCodeSupported)
                {
                    ApplyPadding(textWriter, text, propertyToken);
                }
                else
                {
                    var ansiTokens = Regex.Matches(text, AnsiEscapeRegex);

                    if (ansiTokens.Count == 0)
                    {
                        ApplyPadding(textWriter, text, propertyToken);
                        return;
                    }

                    var textChars = text.ToCharArray();
                    var textCharsLength = textChars.Length;
                    var currentIndex = 0;

                    foreach (Match ansiToken in ansiTokens)
                    {
                        var ansiTokenIndex = ansiToken.Index;
                        var ansiTokenIndexOffset = ansiTokenIndex - currentIndex;
                        var ansiTokenLength = ansiToken.Length;

                        if (currentIndex == ansiTokenIndex)
                        {
                            currentIndex += ansiTokenLength;
                        }
                        else if (currentIndex < ansiTokenIndex)
                        {
                            textWriter.Write(textChars, currentIndex, ansiTokenIndexOffset);
                            currentIndex += ansiTokenIndexOffset + ansiTokenLength;
                        }

                        switch (ansiToken.Value)
                        {
                            case "\u001b[30m":
                                Console.ForegroundColor = ConsoleColor.Black;
                                break;

                            case "\u001b[31m":
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                break;

                            case "\u001b[32m":
                                Console.ForegroundColor = ConsoleColor.DarkGreen;
                                break;

                            case "\u001b[33m":
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                break;

                            case "\u001b[34m":
                                Console.ForegroundColor = ConsoleColor.DarkBlue;
                                break;

                            case "\u001b[35m":
                                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                                break;

                            case "\u001b[36m":
                                Console.ForegroundColor = ConsoleColor.DarkCyan;
                                break;

                            case "\u001b[37m":
                                Console.ForegroundColor = ConsoleColor.White;
                                break;

                            case "\u001b[30;1m":
                                Console.ForegroundColor = ConsoleColor.DarkGray;
                                break;

                            case "\u001b[31;1m":
                                Console.ForegroundColor = ConsoleColor.Red;
                                break;

                            case "\u001b[32;1m":
                                Console.ForegroundColor = ConsoleColor.Green;
                                break;

                            case "\u001b[33;1m":
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;

                            case "\u001b[34;1m":
                                Console.ForegroundColor = ConsoleColor.Blue;
                                break;

                            case "\u001b[35;1m":
                                Console.ForegroundColor = ConsoleColor.Magenta;
                                break;

                            case "\u001b[36;1m":
                                Console.ForegroundColor = ConsoleColor.Cyan;
                                break;

                            case "\u001b[37;1m":
                                Console.ForegroundColor = ConsoleColor.White;
                                break;

                            case "\u001b[40m":
                                Console.BackgroundColor = ConsoleColor.Black;
                                break;

                            case "\u001b[41m":
                                Console.BackgroundColor = ConsoleColor.DarkRed;
                                break;

                            case "\u001b[42m":
                                Console.BackgroundColor = ConsoleColor.DarkGreen;
                                break;

                            case "\u001b[43m":
                                Console.BackgroundColor = ConsoleColor.DarkYellow;
                                break;

                            case "\u001b[44m":
                                Console.BackgroundColor = ConsoleColor.DarkBlue;
                                break;

                            case "\u001b[45m":
                                Console.BackgroundColor = ConsoleColor.DarkMagenta;
                                break;

                            case "\u001b[46m":
                                Console.BackgroundColor = ConsoleColor.DarkCyan;
                                break;

                            case "\u001b[47m":
                                Console.BackgroundColor = ConsoleColor.White;
                                break;

                            case "\u001b[40;1m":
                                Console.BackgroundColor = ConsoleColor.DarkGray;
                                break;

                            case "\u001b[41;1m":
                                Console.BackgroundColor = ConsoleColor.Red;
                                break;

                            case "\u001b[42;1m":
                                Console.BackgroundColor = ConsoleColor.Green;
                                break;

                            case "\u001b[43;1m":
                                Console.BackgroundColor = ConsoleColor.Yellow;
                                break;

                            case "\u001b[44;1m":
                                Console.BackgroundColor = ConsoleColor.Blue;
                                break;

                            case "\u001b[45;1m":
                                Console.BackgroundColor = ConsoleColor.Magenta;
                                break;

                            case "\u001b[46;1m":
                                Console.BackgroundColor = ConsoleColor.Cyan;
                                break;

                            case "\u001b[47;1m":
                                Console.BackgroundColor = ConsoleColor.White;
                                break;

                            case "\u001b[0m":
                                Console.ForegroundColor = DefaultForegroundColor;
                                Console.BackgroundColor = DefaultBackgroundColor;
                                break;
                        }
                    }

                    if (currentIndex < textCharsLength)
                    {
                        textWriter.Write(textChars, currentIndex, textCharsLength - currentIndex);
                    }
                }
            }
            else
            {
                ApplyPadding(textWriter, Regex.Replace(text, AnsiEscapeRegex, string.Empty), propertyToken);
            }
        }

        private static void ApplyPadding(TextWriter textWriter, string text, PropertyToken propertyToken)
        {
            var textLength = text.Length;

            if (propertyToken?.Alignment == null || textLength >= propertyToken.Alignment.Value.Width)
            {
                textWriter.Write(text);
            }
            else
            {
                var alignment = propertyToken.Alignment.Value;
                var amountToPad = alignment.Width - textLength;

                if (alignment.Direction == AlignmentDirection.Left)
                {
                    textWriter.Write(text);
                }

                var padChars = PadChars;

                if (amountToPad <= padChars.Length)
                {
                    textWriter.Write(padChars, 0, amountToPad);
                }
                else
                {
                    textWriter.Write(new string(' ', amountToPad));
                }

                if (alignment.Direction == AlignmentDirection.Right)
                {
                    textWriter.Write(text);
                }
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int handleId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool GetConsoleMode(IntPtr handle, out uint mode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleMode(IntPtr handle, uint mode);
    }
}
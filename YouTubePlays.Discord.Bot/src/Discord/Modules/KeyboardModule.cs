using System;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using YouTubePlays.Discord.Bot.Discord.Command;
using YouTubePlays.Discord.Bot.Keyboard;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    [Name("Keyboard")]
    public class KeyboardModule : AbstractModule
    {
        private readonly KeyboardCollection _keyboardCollection;
        private readonly ChatBot _chatBot;

        public KeyboardModule(KeyboardCollection keyboardCollection, ChatBot chatBot)
        {
            _keyboardCollection = keyboardCollection;
            _chatBot = chatBot;
        }

        [Command("GetAvailableKeyboard")]
        [Alias("Keyboards")]
        [Summary("Get all the available keyboards.")]
        public async Task GetAvailableKeyboardAsync()
        {
            var keyboardHelp = new StringBuilder();
            keyboardHelp.AppendLine("Here are the list of available keyboards you can use:");

            foreach (var keyboard in _keyboardCollection.Keyboards)
            {
                keyboardHelp.AppendLine($"{keyboard.ShortKey}: {keyboard.Name}");
            }

            await ReplyAsync(keyboardHelp.ToString()).ConfigureAwait(false);
        }

        [Command("GetCurrentKeyboard", true)]
        [Alias("CurrentKeyboard")]
        [Summary("Get the current keyboard.")]
        public async Task GetCurrentKeyboardAsync()
        {
            await ReplyAsync($"The current keyboard is: {_keyboardCollection.CurrentKeyboard.Name}").ConfigureAwait(false);
        }

        [Command("SetCurrentKeyboard")]
        [Alias("SetKeyboard")]
        [Summary("Set the current keyboard.")]
        public async Task SetCurrentKeyboardAsync([Summary("Keyboard to set.")] [Remainder]
            string keyboardCode)
        {
            var isAvailable = false;

            foreach (var keyboard in _keyboardCollection.Keyboards)
            {
                if (!keyboard.ShortKey.Equals(keyboardCode, StringComparison.OrdinalIgnoreCase)) continue;

                isAvailable = true;
                _keyboardCollection.CurrentKeyboard = keyboard;
                break;
            }

            if (!isAvailable)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid keyboard.").BuildDiscordTextResponse()).ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync(CommandExecuteResult.FromSuccess($"Changed keyboard to: {_keyboardCollection.CurrentKeyboard.Name}").BuildDiscordTextResponse()).ConfigureAwait(false);
            }
        }

        [Command("GetKeyboardCommand")]
        [Alias("Name", "Command")]
        [Summary("Get keyboard command.")]
        public async Task GetKeyboardCommandAsync([Summary("Keys on the keyboard.")] [Remainder]
            string keys)
        {
            var result = _keyboardCollection.GetCommands(keys, _chatBot);

            await ReplyAsync("Here is the command to enter this name in ytp:").ConfigureAwait(false);

            foreach (var command in result)
            {
                await ReplyAsync(command).ConfigureAwait(false);
            }
        }
    }
}
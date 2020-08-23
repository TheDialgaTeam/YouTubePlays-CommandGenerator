using System;
using System.Text;
using System.Threading;
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

        public KeyboardModule(KeyboardCollection keyboardCollection, IServiceProvider serviceProvider, CancellationTokenSource cancellationTokenSource) : base(serviceProvider, cancellationTokenSource)
        {
            _keyboardCollection = keyboardCollection;
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
        [RequireContext(ContextType.Guild)]
        public async Task GetCurrentKeyboardAsync()
        {
            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);

            if (_keyboardCollection.TryGetKeyboard(channelSettings.KeyboardType, out var keyboard))
            {
                await ReplyAsync($"The current keyboard is: {keyboard.Name}").ConfigureAwait(false);
            }
            else
            {
                await ReplyAsync(CommandExecuteResult.FromError("Unable to get the current keyboard.").BuildDiscordTextResponse()).ConfigureAwait(false);
            }
            
        }

        [Command("SetCurrentKeyboard")]
        [Alias("SetKeyboard")]
        [Summary("Set the current keyboard.")]
        [Example("SetCurrentKeyboard 5")]
        [RequireContext(ContextType.Guild)]
        public async Task SetCurrentKeyboardAsync([Summary("Keyboard to set.")] [Remainder]
            string keyboardCode)
        {
            if (!_keyboardCollection.TryGetKeyboard(keyboardCode, out var keyboard))
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid keyboard.").BuildDiscordTextResponse()).ConfigureAwait(false);
            }
            else
            {
                var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
                channelSettings.KeyboardType = keyboard.ShortKey;
                await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

                await ReplyAsync(CommandExecuteResult.FromSuccess($"Changed keyboard to: {keyboard.Name}").BuildDiscordTextResponse()).ConfigureAwait(false);
            }
        }

        [Command("GetKeyboardCommand")]
        [Alias("Name", "Command")]
        [Summary("Get keyboard command.")]
        [Example("GetKeyboardCommand test")]
        [RequireContext(ContextType.Guild)]
        public async Task GetKeyboardCommandAsync([Summary("Keys on the keyboard.")] [Remainder]
            string keys)
        {
            try
            {
                var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
                var result = _keyboardCollection.GetCommands(keys, channelSettings);

                if (result.Length == 0) throw new Exception("Unexpected error occurred.");

                await ReplyAsync("Here is the command to enter this name in ytp:").ConfigureAwait(false);

                foreach (var command in result)
                {
                    await ReplyAsync(command).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                await ReplyAsync(CommandExecuteResult.FromError(ex.Message).BuildDiscordTextResponse()).ConfigureAwait(false);
            }
        }
    }
}
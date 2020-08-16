using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using YouTubePlays.Discord.Bot.Discord.Command;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    [Name("Chat Bot")]
    public class ChatBotModule : AbstractModule
    {
        private readonly ChatBot _chatBot;

        public ChatBotModule(ChatBot chatBot)
        {
            _chatBot = chatBot;
        }

        [Command("GetChatBotConfig", true)]
        [Summary("Get current chat bot configuration.")]
        public async Task GetChatBotConfigAsync()
        {
            var config = new EmbedBuilder()
                .WithTitle("Current Chat Bot Configuration:")
                .WithColor(Color.Green)
                .WithDescription($@"Input Limit: {_chatBot.InputLimit}
Touch Availability: {_chatBot.TouchAvailable}
Touch X Offset: {_chatBot.TouchXOffset}
Touch Y Offset: {_chatBot.TouchYOffset}")
                .Build();

            await ReplyAsync(null, false, config).ConfigureAwait(false);
        }

        [Command("SetChatBotInputLimit")]
        [Summary("Set chat bot input limit.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task SetChatBotInputLimitAsync([Summary("Input limit to conform to.")]
            int inputLimit)
        {
            if (inputLimit < 1)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            _chatBot.InputLimit = inputLimit;
            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed input limit to {inputLimit}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("EnableChatBotTouch", true)]
        [Summary("Enable chat bot touch commands.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task EnableChatBotTouchAsync()
        {
            _chatBot.TouchAvailable = true;
            await ReplyAsync(CommandExecuteResult.FromSuccess("Successfully enabled touch commands.").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("DisableChatBotTouch", true)]
        [Summary("Disable chat bot touch commands.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task DisableChatBotTouchAsync()
        {
            _chatBot.TouchAvailable = false;
            await ReplyAsync(CommandExecuteResult.FromSuccess("Successfully disabled touch commands.").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("SetChatBotTouchXOffset")]
        [Summary("Set chat bot touch x coordinate offset.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task SetChatBotTouchXOffset([Summary("X coordinate offset.")] int offset)
        {
            if (offset < -100 || offset > 100)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            _chatBot.TouchXOffset = offset;
            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed touch x coordinate offset to {offset}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("SetChatBotTouchYOffset")]
        [Summary("Set chat bot touch y coordinate offset.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task SetChatBotTouchYOffset([Summary("Y coordinate offset.")] int offset)
        {
            if (offset < -100 || offset > 100)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            _chatBot.TouchYOffset = offset;
            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed touch y coordinate offset to {offset}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }
    }
}
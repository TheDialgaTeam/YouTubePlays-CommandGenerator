using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.Hosting;
using YouTubePlays.Discord.Bot.Discord.Command;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    [Name("Chat Bot")]
    [RequireContext(ContextType.Guild)]
    public class ChatBotModule : AbstractSqliteContextModule
    {
        public ChatBotModule(IHostApplicationLifetime hostApplicationLifetime) : base(hostApplicationLifetime)
        {
        }

        [Command("GetChatBotConfig", true)]
        [Summary("Get current chat bot configuration.")]
        public async Task GetChatBotConfigAsync()
        {
            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);

            var config = new EmbedBuilder()
                .WithTitle("Current Chat Bot Configuration:")
                .WithColor(Color.Green)
                .WithDescription($@"Input Limit: {channelSettings.InputLimit}
Touch Availability: {channelSettings.TouchAvailable}
Touch X Offset: {channelSettings.TouchXOffset}
Touch Y Offset: {channelSettings.TouchYOffset}")
                .Build();

            await ReplyAsync(config).ConfigureAwait(false);
        }

        [Command("SetChatBotInputLimit")]
        [Summary("Set chat bot input limit.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        [Example("SetChatBotInputLimit 5")]
        public async Task SetChatBotInputLimitAsync([Summary("Input limit to conform to.")]
            int inputLimit)
        {
            if (inputLimit < 1)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
            channelSettings.InputLimit = inputLimit;
            await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed input limit to {inputLimit}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("EnableChatBotTouch", true)]
        [Summary("Enable chat bot touch commands.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task EnableChatBotTouchAsync()
        {
            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
            channelSettings.TouchAvailable = true;
            await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

            await ReplyAsync(CommandExecuteResult.FromSuccess("Successfully enabled touch commands.").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("DisableChatBotTouch", true)]
        [Summary("Disable chat bot touch commands.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        public async Task DisableChatBotTouchAsync()
        {
            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
            channelSettings.TouchAvailable = false;
            await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

            await ReplyAsync(CommandExecuteResult.FromSuccess("Successfully disabled touch commands.").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("SetChatBotTouchXOffset")]
        [Summary("Set chat bot touch x coordinate offset.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        [Example("SetChatBotTouchXOffset 0")]
        public async Task SetChatBotTouchXOffset([Summary("X coordinate offset.")] int offset)
        {
            if (offset < -100 || offset > 100)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
            channelSettings.TouchXOffset = offset;
            await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed touch x coordinate offset to {offset}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }

        [Command("SetChatBotTouchYOffset")]
        [Summary("Set chat bot touch y coordinate offset.")]
        [RequirePermission(RequiredPermission.GuildAdministrator)]
        [Example("SetChatBotTouchYOffset 0")]
        public async Task SetChatBotTouchYOffset([Summary("Y coordinate offset.")] int offset)
        {
            if (offset < -100 || offset > 100)
            {
                await ReplyAsync(CommandExecuteResult.FromError("Invalid input.").BuildDiscordTextResponse()).ConfigureAwait(false);
                return;
            }

            var channelSettings = await GetChannelSettingsAsync().ConfigureAwait(false);
            channelSettings.TouchYOffset = offset;
            await SqliteContext.SaveChangesAsync().ConfigureAwait(false);

            await ReplyAsync(CommandExecuteResult.FromSuccess($"Successfully changed touch y coordinate offset to {offset}").BuildDiscordTextResponse()).ConfigureAwait(false);
        }
    }
}
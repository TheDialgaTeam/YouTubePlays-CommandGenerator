﻿using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    public abstract class AbstractModule : ModuleBase<ShardedCommandContext>
    {
        protected override async Task<IUserMessage?> ReplyAsync(string? message = null, bool isTTS = false, Embed? embed = null, RequestOptions? options = null)
        {
            if (Context.Message.Channel is SocketDMChannel || Context.Message.Channel is SocketGroupChannel)
            {
                return await Context.Channel.SendMessageAsync(message, isTTS, embed, options).ConfigureAwait(false);
            }

            if (GetChannelPermissions().SendMessages)
            {
                return await Context.Channel.SendMessageAsync(message, isTTS, embed, options).ConfigureAwait(false);
            }

            return null;
        }

        protected async Task<IUserMessage?> ReplyDMAsync(string text, bool isTTS = false, Embed? embed = null, RequestOptions? options = null)
        {
            if (Context.Message.Channel is SocketDMChannel || Context.Message.Channel is SocketGroupChannel)
            {
                return await ReplyAsync(text, isTTS, embed, options).ConfigureAwait(false);
            }

            var dmChannel = await Context.Message.Author.GetOrCreateDMChannelAsync().ConfigureAwait(false);
            return await dmChannel.SendMessageAsync(text, isTTS, embed, options).ConfigureAwait(false);
        }

        protected ChannelPermissions GetChannelPermissions(ulong? channelId = null)
        {
            return Context.Guild.GetUser(Context.Client.CurrentUser.Id).GetPermissions(Context.Guild.GetChannel(channelId ?? Context.Channel.Id));
        }
    }
}
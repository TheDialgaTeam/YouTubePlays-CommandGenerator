﻿using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    public abstract class AbstractModule : ModuleBase<ShardedCommandContext>
    {
        private readonly RequestOptions _requestOptions;

        protected AbstractModule(CancellationTokenSource cancellationTokenSource)
        {
            _requestOptions = new RequestOptions { CancelToken = cancellationTokenSource.Token };
        }

        protected async Task<IUserMessage?> ReplyAsync(string message, bool isTTS = false)
        {
            var channelContext = Context.Channel;
            
            if (Context.Message.Channel is SocketDMChannel)
            {
                return await channelContext.SendMessageAsync(message, isTTS, null, _requestOptions).ConfigureAwait(false);
            }

            var guildContext = Context.Guild;

            if (guildContext.GetUser(Context.Client.CurrentUser.Id).GetPermissions(guildContext.GetChannel(channelContext.Id)).SendMessages)
            {
                return await channelContext.SendMessageAsync(message, isTTS, null, _requestOptions).ConfigureAwait(false);
            }

            return null;
        }

        protected async Task<IUserMessage?> ReplyAsync(Embed embed)
        {
            var channelContext = Context.Channel;
            
            if (Context.Message.Channel is SocketDMChannel)
            {
                return await channelContext.SendMessageAsync(null, false, embed, _requestOptions).ConfigureAwait(false);
            }

            var guildContext = Context.Guild;

            if (guildContext.GetUser(Context.Client.CurrentUser.Id).GetPermissions(guildContext.GetChannel(channelContext.Id)).SendMessages)
            {
                return await channelContext.SendMessageAsync(null, false, embed, _requestOptions).ConfigureAwait(false);
            }

            return null;
        }

        protected async Task<IUserMessage> ReplyToDMAsync(string text, bool isTTS = false)
        {
            var messageContext = Context.Message;

            if (messageContext.Channel is SocketDMChannel)
            {
                return await ReplyAsync(text, isTTS, null, _requestOptions).ConfigureAwait(false);
            }

            var dmChannel = await messageContext.Author.GetOrCreateDMChannelAsync(_requestOptions).ConfigureAwait(false);
            return await dmChannel.SendMessageAsync(text, isTTS, null, _requestOptions).ConfigureAwait(false);
        }

        protected async Task<IUserMessage> ReplyToDMAsync(Embed embed)
        {
            var messageContext = Context.Message;

            if (messageContext.Channel is SocketDMChannel)
            {
                return await ReplyAsync(null, false, embed, _requestOptions).ConfigureAwait(false);
            }

            var dmChannel = await messageContext.Author.GetOrCreateDMChannelAsync(_requestOptions).ConfigureAwait(false);
            return await dmChannel.SendMessageAsync(null, false, embed, _requestOptions).ConfigureAwait(false);
        }
    }
}
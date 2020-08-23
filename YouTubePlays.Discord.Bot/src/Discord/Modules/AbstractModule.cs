using System;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using YouTubePlays.Discord.Bot.EntityFramework;
using YouTubePlays.Discord.Bot.EntityFramework.Table;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    public abstract class AbstractModule : ModuleBase<ShardedCommandContext>
    {
        protected SqliteContext SqliteContext;
        protected CancellationToken CancellationToken;

        private readonly IServiceScope _serviceScope;
        private readonly RequestOptions _requestOptions;

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        protected AbstractModule(IServiceProvider serviceProvider, CancellationTokenSource cancellationTokenSource)
        {
            _serviceScope = serviceProvider.CreateScope();
            CancellationToken = cancellationTokenSource.Token;
            _requestOptions = new RequestOptions { CancelToken = cancellationTokenSource.Token };
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected override void BeforeExecute(CommandInfo command)
        {
            SqliteContext = _serviceScope.ServiceProvider.GetRequiredService<SqliteContext>();
        }

        protected override void AfterExecute(CommandInfo command)
        {
            _serviceScope.Dispose();
        }

        protected async Task<ChannelSettings> GetChannelSettingsAsync()
        {
            var channelSettings = await SqliteContext.ChannelSettingsTable.FindAsync(Context.Channel.Id).ConfigureAwait(false);
            if (channelSettings != null) return channelSettings;

            channelSettings = new ChannelSettings { ChannelId = Context.Channel.Id };

            await SqliteContext.ChannelSettingsTable.AddAsync(channelSettings, CancellationToken).ConfigureAwait(false);
            await SqliteContext.SaveChangesAsync(CancellationToken).ConfigureAwait(false);

            return channelSettings;
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
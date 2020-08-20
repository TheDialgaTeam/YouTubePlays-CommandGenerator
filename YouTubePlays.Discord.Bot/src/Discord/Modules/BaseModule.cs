using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    [Name("Base")]
    public class BaseModule : AbstractModule
    {
        public BaseModule(CancellationTokenSource cancellationTokenSource) : base(cancellationTokenSource)
        {
        }

        [Command("Ping", true)]
        [Summary("Gets the estimated round-trip latency, in milliseconds, to the gateway server.")]
        public async Task PingAsync()
        {
            await ReplyAsync($"Ping: {Context.Client.Latency} ms").ConfigureAwait(false);
        }

        [Command("About", true)]
        [Summary("Get the bot information.")]
        public async Task AboutAsync()
        {
            var clientContext = Context.Client;
            var applicationInfo = await clientContext.GetApplicationInfoAsync().ConfigureAwait(false);
            var currentUser = clientContext.CurrentUser;

            var helpMessage = new EmbedBuilder()
                .WithTitle("YouTubePlays Command Generator Bot:")
                .WithThumbnailUrl(currentUser.GetAvatarUrl())
                .WithColor(Color.Orange)
                .WithDescription($@"Hello, I am **{currentUser.Username}**, a YouTubePlays command generator bot that is created by jianmingyong#4964.

I am owned by **{applicationInfo.Owner}**.

Type `@{clientContext.CurrentUser} help` to see my command. You can also type `help` in this DM to see any command that can be used in this DM.");

            await ReplyToDMAsync(helpMessage.Build()).ConfigureAwait(false);
        }
    }
}
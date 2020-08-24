using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using YouTubePlays.Discord.Bot.EntityFramework;
using YouTubePlays.Discord.Bot.EntityFramework.Table;

namespace YouTubePlays.Discord.Bot.Discord.Modules
{
    public class AbstractSqliteContextModule : AbstractModule
    {
        protected SqliteContext SqliteContext = null!;

        protected AbstractSqliteContextModule(IHostApplicationLifetime hostApplicationLifetime) : base(hostApplicationLifetime)
        {
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            SqliteContext = Context.ServiceProvider.GetRequiredService<SqliteContext>();
        }

        protected async Task<ChannelSettings> GetChannelSettingsAsync()
        {
            var channelSettings = await SqliteContext.ChannelSettingsTable.FindAsync(Context.Channel.Id).ConfigureAwait(false);
            if (channelSettings != null) return channelSettings;

            channelSettings = new ChannelSettings { ChannelId = Context.Channel.Id };

            await SqliteContext.ChannelSettingsTable.AddAsync(channelSettings, HostApplicationLifetime.ApplicationStopping).ConfigureAwait(false);
            await SqliteContext.SaveChangesAsync(HostApplicationLifetime.ApplicationStopping).ConfigureAwait(false);

            return channelSettings;
        }
    }
}
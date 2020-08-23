using Microsoft.EntityFrameworkCore;
using YouTubePlays.Discord.Bot.EntityFramework.Table;

namespace YouTubePlays.Discord.Bot.EntityFramework
{
    public class SqliteContext : DbContext
    {
        public DbSet<ChannelSettings> ChannelSettingsTable { get; set; }

#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
        public SqliteContext(DbContextOptions<SqliteContext> context) : base(context)
        {
        }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
    }
}
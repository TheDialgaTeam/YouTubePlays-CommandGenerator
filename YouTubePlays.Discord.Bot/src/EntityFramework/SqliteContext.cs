using Microsoft.EntityFrameworkCore;
using YouTubePlays.Discord.Bot.EntityFramework.Table;

namespace YouTubePlays.Discord.Bot.EntityFramework
{
    public class SqliteContext : DbContext
    {
        public DbSet<ChannelSettings> ChannelSettingsTable { get; set; } = null!;

        public SqliteContext(DbContextOptions<SqliteContext> context) : base(context)
        {
        }
    }
}
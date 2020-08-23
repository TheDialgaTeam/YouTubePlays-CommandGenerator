using Microsoft.EntityFrameworkCore.Migrations;

namespace YouTubePlays.Discord.Bot.Migrations
{
    public partial class _0200 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelSettingsTable",
                columns: table => new
                {
                    ChannelId = table.Column<ulong>(nullable: false),
                    KeyboardType = table.Column<string>(nullable: false),
                    InputLimit = table.Column<int>(nullable: false),
                    TouchAvailable = table.Column<bool>(nullable: false),
                    TouchXOffset = table.Column<int>(nullable: false),
                    TouchYOffset = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelSettingsTable", x => x.ChannelId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChannelSettingsTable");
        }
    }
}

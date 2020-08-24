using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TheDialgaTeam.Core.Logger;
using TheDialgaTeam.Core.Logger.Formatter;
using YouTubePlays.Discord.Bot.Discord;
using YouTubePlays.Discord.Bot.EntityFramework;
using YouTubePlays.Discord.Bot.Keyboard;

namespace YouTubePlays.Discord.Bot
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddSingleton<KeyboardCollection>();

                    serviceCollection.AddDbContext<SqliteContext>((provider, builder) =>
                    {
                        var hostEnvironment = provider.GetRequiredService<IHostEnvironment>();
                        builder.UseSqlite($"Data Source={Path.Combine(hostEnvironment.ContentRootPath, "data.db")}");
                    });

                    serviceCollection.AddDiscordShardedAppContext(new DiscordSocketConfig { LogLevel = LogSeverity.Verbose });

                    serviceCollection.AddHostedService<ProgramHostedService>();
                    serviceCollection.AddHostedService<DiscordAppHostedService>();
                })
                .UseSerilog((hostBuilderContext, serviceProvider, loggerConfiguration) =>
                {
                    var logsDirectory = Path.Combine(hostBuilderContext.HostingEnvironment.ContentRootPath, "Logs");
                    if (!Directory.Exists(logsDirectory)) Directory.CreateDirectory(logsDirectory);

                    const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Message}{NewLine}{Exception}";

                    loggerConfiguration
                        .ReadFrom.Configuration(hostBuilderContext.Configuration)
                        .WriteTo.AnsiConsole(new OutputTemplateTextFormatter(outputTemplate))
                        .WriteTo.Conditional(logEvent => hostBuilderContext.HostingEnvironment.IsProduction(), loggerSinkConfiguration => { loggerSinkConfiguration.Async(sinkConfiguration => { sinkConfiguration.File(Path.Combine(logsDirectory, "log.log"), outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null); }, blockWhenFull: true); });
                });
        }
    }
}
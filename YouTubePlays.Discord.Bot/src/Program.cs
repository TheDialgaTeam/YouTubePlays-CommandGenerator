using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using TheDialgaTeam.Core.Logger;
using TheDialgaTeam.Core.Logger.Formatter;
using YouTubePlays.Discord.Bot.Discord;
using YouTubePlays.Discord.Bot.Discord.Command;
using YouTubePlays.Discord.Bot.EntityFramework;
using YouTubePlays.Discord.Bot.Keyboard;

namespace YouTubePlays.Discord.Bot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var cancellationToken = host.Services.GetRequiredService<CancellationTokenSource>().Token;
            await host.RunAsync(cancellationToken).ConfigureAwait(false);
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, serviceCollection) =>
                {
                    serviceCollection.AddSingleton<CancellationTokenSource>();

                    serviceCollection.AddSingleton<KeyboardCollection>();

                    serviceCollection.AddDbContext<SqliteContext>((provider, builder) =>
                    {
                        var hostEnvironment = provider.GetRequiredService<IHostEnvironment>();
                        builder.UseSqlite($"Data Source={Path.Combine(hostEnvironment.ContentRootPath, "data.db")}");
                    });

                    serviceCollection.AddSingleton(serviceProvider =>
                    {
                        var commandService = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false });
                        commandService.AddTypeReader<IEmote>(new EmoteTypeReader());
                        commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), serviceProvider);
                        return commandService;
                    });

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
                })
                .UseConsoleLifetime();
        }

        //private static void Main()
        //{
        //    DependencyManager.InstallServices(serviceCollection =>
        //    {
        //        serviceCollection.AddSingleton<IConfig, JsonConfig>(factory => new JsonConfig(Path.Combine(Environment.CurrentDirectory, "config.json")));

        //        serviceCollection.AddDbContext<SqliteContext>(builder =>
        //        {
        //            var dataDirectory = Path.Combine(Environment.CurrentDirectory, "Data");

        //            if (!Directory.Exists(dataDirectory))
        //            {
        //                Directory.CreateDirectory(dataDirectory);
        //            }

        //            builder.UseSqlite($"Data Source={Path.Combine(dataDirectory, "Data.db")}");
        //        });

        //        serviceCollection.AddSingleton(factory => new LoggingLevelSwitch(LogEventLevel.Verbose));
        //        serviceCollection.AddSingleton<ILogger>(factory =>
        //        {
        //            const string outputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Message}{NewLine}{Exception}";

        //            var logsDirectory = Path.Combine(Environment.CurrentDirectory, "Logs");

        //            if (!Directory.Exists(logsDirectory))
        //            {
        //                Directory.CreateDirectory(logsDirectory);
        //            }

        //            return new LoggerConfiguration()
        //                .MinimumLevel.ControlledBy(factory.GetRequiredService<LoggingLevelSwitch>())
        //                .WriteTo.AnsiConsole(new OutputTemplateTextFormatter(outputTemplate))
        //                .WriteTo.Async(configuration => configuration.File(Path.Combine(logsDirectory, "log.log"), outputTemplate: outputTemplate, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: null, retainedFileCountLimit: null), blockWhenFull: true)
        //                .CreateLogger();
        //        });

        //        serviceCollection.AddSingleton(factory =>
        //        {
        //            var commandService = new CommandService(new CommandServiceConfig { CaseSensitiveCommands = false });
        //            commandService.AddTypeReader<IEmote>(new EmoteTypeReader());
        //            commandService.AddModulesAsync(Assembly.GetExecutingAssembly(), factory);

        //            return commandService;
        //        });

        //        serviceCollection.AddSingleton<ChatBot>();
        //        serviceCollection.AddSingleton<KeyboardCollection>();

        //        serviceCollection.AddSingleton<IServiceExecutor, BootstrapServiceExecutor>();
        //        serviceCollection.AddSingleton<IServiceExecutor, ConsoleServiceExecutor>();
        //        serviceCollection.AddSingleton<IServiceExecutor, DiscordAppServiceExecutor>();
        //    });

        //    DependencyManager.BuildAndExecute((serviceProvider, ex) =>
        //    {
        //        if (serviceProvider != null)
        //        {
        //            var cancellationTokenSource = serviceProvider.GetService<CancellationTokenSource>();
        //            var logger = serviceProvider.GetService<ILogger>();

        //            if (logger != null)
        //            {
        //                if (ex is AggregateException aggregateException)
        //                {
        //                    var innerExceptions = aggregateException.InnerExceptions;

        //                    foreach (var exception in innerExceptions)
        //                    {
        //                        if (exception is TaskCanceledException) continue;
        //                        logger.Fatal(exception, "\u001b[31;1mProgram crashed!\u001b[0m");
        //                    }
        //                }
        //                else
        //                {
        //                    logger.Fatal(ex, "\u001b[31;1mProgram crashed!\u001b[0m");
        //                }
        //            }

        //            cancellationTokenSource?.Cancel();
        //        }
        //        else
        //        {
        //            System.Console.Error.WriteLine(ex.ToString());
        //        }

        //        Environment.Exit(1);
        //    });
        //}
    }
}
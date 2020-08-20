using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using Serilog;
using TheDialgaTeam.Core.DependencyInjection;
using YouTubePlays.Discord.Bot.Config;

namespace YouTubePlays.Discord.Bot
{
    public class BootstrapServiceExecutor : IServiceExecutor
    {
        private readonly ILogger _logger;
        private readonly IConfig _config;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public BootstrapServiceExecutor(ILogger logger, IConfig config, CancellationTokenSource cancellationTokenSource)
        {
            _logger = logger;
            _config = config;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void ExecuteService(ITaskCreator taskCreator)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var frameworkVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            System.Console.Title = $"YouTubePlays Command Generator Bot v{version} ({frameworkVersion})";
            System.Console.CancelKeyPress += (sender, args) => { args.Cancel = true; };

            _logger.Information("==================================================");
            _logger.Information("YouTubePlays Command Generator Bot v{version:l} {frameworkVersion:l}", version, frameworkVersion);
            _logger.Information("==================================================");

            if (File.Exists(_config.ConfigFilePath))
            {
                _config.LoadConfig();
                _logger.Information("Config loaded.");
            }
            else
            {
                _config.SaveConfig();
                _logger.Information("Config saved at: {ConfigFilePath}", _config.ConfigFilePath);
                _logger.Information("Please configure the bot before relaunch :)");
                _logger.Information("Press enter key to exit.");
                System.Console.ReadLine();
                _cancellationTokenSource.Cancel();
            }
        }
    }
}
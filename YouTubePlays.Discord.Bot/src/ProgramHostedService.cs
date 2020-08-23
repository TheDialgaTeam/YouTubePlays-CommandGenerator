using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using YouTubePlays.Discord.Bot.EntityFramework;

namespace YouTubePlays.Discord.Bot
{
    public class ProgramHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ProgramHostedService> _logger;

        public ProgramHostedService(IServiceProvider serviceProvider, ILogger<ProgramHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            var frameworkVersion = Assembly.GetExecutingAssembly().GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

            System.Console.Title = $"YouTubePlays Command Generator Bot v{version} ({frameworkVersion})";

            _logger.LogInformation("==================================================");
            _logger.LogInformation("YouTubePlays Command Generator Bot v{version:l} {frameworkVersion:l}", version, frameworkVersion);
            _logger.LogInformation("==================================================");

            _logger.LogInformation("Checking for database update...");

            using (var scope = _serviceProvider.CreateScope())
            {
                var sqliteContext = scope.ServiceProvider.GetRequiredService<SqliteContext>();
                await sqliteContext.Database.MigrateAsync(cancellationToken).ConfigureAwait(false);
            }

            _logger.LogInformation("\u001b[32;1mDatabase initialized!\u001b[0m");
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Application shutdown!");
            return Task.CompletedTask;
        }
    }
}

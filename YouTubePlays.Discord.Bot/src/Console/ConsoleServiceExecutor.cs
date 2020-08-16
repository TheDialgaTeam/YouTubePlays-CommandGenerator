using System;
using System.Reflection;
using System.Runtime.Versioning;
using System.Threading;
using Serilog;
using TheDialgaTeam.Core.DependencyInjection;

namespace YouTubePlays.Discord.Bot.Console
{
    public class ConsoleServiceExecutor : IServiceExecutor
    {
        private readonly ILogger _logger;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public ConsoleServiceExecutor(ILogger logger, CancellationTokenSource cancellationTokenSource)
        {
            _logger = logger;
            _cancellationTokenSource = cancellationTokenSource;
        }

        public void ExecuteService(ITaskCreator taskCreator)
        {
            taskCreator.CreateAndEnqueueTask((_logger, _cancellationTokenSource), async (state, cancellationToken) =>
            {
                var (logger, cancellationTokenSource) = state;

                while (!cancellationToken.IsCancellationRequested)
                {
                    var command = await System.Console.In.ReadLineAsync().ConfigureAwait(false);

                    if (command == null)
                    {
                        cancellationTokenSource.Cancel();
                        continue;
                    }

                    command = command.Trim();

                    if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
                    {
                        logger.Information("Available commands:");
                        logger.Information("Exit - Shutdown the bot.");
                    }
                    else if (command.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        cancellationTokenSource.Cancel();
                    }
                    else
                    {
                        logger.Information("Invalid Command!");
                    }
                }
            });
        }
    }
}
using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace TheDialgaTeam.Core.DependencyInjection
{
    public class DependencyManager : IDisposable
    {
        private readonly IServiceCollection _serviceCollection = new ServiceCollection();

        private ServiceProvider? _serviceProvider;

        public DependencyManager()
        {
            _serviceCollection.AddSingleton(factory => factory);
            _serviceCollection.AddSingleton<CancellationTokenSource>();
            _serviceCollection.AddSingleton<ITaskCreator, TaskCollection>();
        }

        public void InstallServices(Action<IServiceCollection> installServiceAction)
        {
            installServiceAction(_serviceCollection);
        }

        public void BuildAndExecute(Action<IServiceProvider?, Exception> executeFailedAction)
        {
            try
            {
                _serviceProvider = _serviceCollection.BuildServiceProvider();

                var serviceExecutors = _serviceProvider.GetServices<IServiceExecutor>();

                if (serviceExecutors != null)
                {
                    var taskCreator = _serviceProvider.GetRequiredService<ITaskCreator>();

                    foreach (var serviceExecutor in serviceExecutors)
                    {
                        serviceExecutor.ExecuteService(taskCreator);
                    }
                }

                (_serviceProvider.GetRequiredService<ITaskCreator>() as TaskCollection)?.WaitAll();
            }
            catch (Exception ex)
            {
                executeFailedAction(_serviceProvider, ex);
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
        }
    }
}
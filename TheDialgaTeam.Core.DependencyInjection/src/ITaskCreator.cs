using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheDialgaTeam.Core.DependencyInjection
{
    public interface ITaskCreator
    {
        Task CreateAndEnqueueTask(Action<CancellationToken> taskToAwait);

        Task CreateAndEnqueueTask(Func<CancellationToken, Task> taskToAwait);

        Task CreateAndEnqueueTask<T>(T state, Action<T, CancellationToken> action);

        Task CreateAndEnqueueTask<T>(T state, Func<T, CancellationToken, Task> function);
    }
}
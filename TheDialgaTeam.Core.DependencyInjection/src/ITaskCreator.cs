using System;
using System.Threading;
using System.Threading.Tasks;

namespace TheDialgaTeam.Core.DependencyInjection
{
    public interface ITaskCreator
    {
        void CreateAndEnqueueTask(Action<CancellationToken> taskToAwait);

        void CreateAndEnqueueTask(Func<CancellationToken, Task> taskToAwait);

        void CreateAndEnqueueTask<T>(T state, Action<T, CancellationToken> action);

        void CreateAndEnqueueTask<T>(T state, Func<T, CancellationToken, Task> function);
    }
}
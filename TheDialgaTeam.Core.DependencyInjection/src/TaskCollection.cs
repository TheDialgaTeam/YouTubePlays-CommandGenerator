using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TheDialgaTeam.Core.DependencyInjection
{
    internal class TaskCollection : ITaskCreator, IDisposable
    {
        private readonly CancellationToken _cancellationToken;

        private readonly List<Task> _taskToAwait = new List<Task>();

        private bool _isDisposed;

        public TaskCollection(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationToken = cancellationTokenSource.Token;
        }

        public Task CreateAndEnqueueTask(Action<CancellationToken> action)
        {
            var task = Task.Factory.StartNew(state =>
            {
                if (state is ValueTuple<Action<CancellationToken>, CancellationToken> innerState)
                {
                    innerState.Item1(innerState.Item2);
                }
            }, (action, _cancellationToken), _cancellationToken);

            _taskToAwait.Add(task);

            return task;
        }

        public Task CreateAndEnqueueTask(Func<CancellationToken, Task> function)
        {
            var task = Task.Factory.StartNew(async state =>
            {
                if (state is ValueTuple<Func<CancellationToken, Task>, CancellationToken> innerState)
                {
                    await innerState.Item1(innerState.Item2).ConfigureAwait(false);
                }
            }, (function, _cancellationToken), _cancellationToken).Unwrap();

            _taskToAwait.Add(task);

            return task;
        }

        public Task CreateAndEnqueueTask<T>(T state, Action<T, CancellationToken> action)
        {
            var task = Task.Factory.StartNew(state2 =>
            {
                if (state2 is ValueTuple<T, Action<T, CancellationToken>, CancellationToken> innerState)
                {
                    innerState.Item2(innerState.Item1, innerState.Item3);
                }
            }, (state, action, _cancellationToken), _cancellationToken);

            _taskToAwait.Add(task);

            return task;
        }

        public Task CreateAndEnqueueTask<T>(T state, Func<T, CancellationToken, Task> function)
        {
            var task = Task.Factory.StartNew(async state2 =>
            {
                if (state2 is ValueTuple<T, Func<T, CancellationToken, Task>, CancellationToken> innerState)
                {
                    await innerState.Item2(innerState.Item1, innerState.Item3).ConfigureAwait(false);
                }
            }, (state, function, _cancellationToken), _cancellationToken).Unwrap();

            _taskToAwait.Add(task);

            return task;
        }

        public void WaitAll()
        {
            Task.WaitAll(_taskToAwait.ToArray());
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;

            var taskToAwait = _taskToAwait;

            foreach (var task in taskToAwait)
            {
                task.Dispose();
            }
        }
    }
}
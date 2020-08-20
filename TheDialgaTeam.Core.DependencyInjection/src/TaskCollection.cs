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

        public TaskCollection(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationToken = cancellationTokenSource.Token;
        }

        public void CreateAndEnqueueTask(Action<CancellationToken> action)
        {
            _taskToAwait.Add(Task.Factory.StartNew(state =>
            {
                if (state is (Action<CancellationToken> stateAction, CancellationToken stateCancellationToken))
                {
                    stateAction(stateCancellationToken);
                }
            }, (action, _cancellationToken), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
        }

        public void CreateAndEnqueueTask(Func<CancellationToken, Task> function)
        {
            _taskToAwait.Add(Task.Factory.StartNew( state => state is (Func<CancellationToken, Task> stateFunction, CancellationToken stateCancellationToken) ? stateFunction(stateCancellationToken) : Task.CompletedTask, (function, _cancellationToken), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap());
        }

        public void CreateAndEnqueueTask<T>(T state, Action<T, CancellationToken> action)
        {
            _taskToAwait.Add(Task.Factory.StartNew(innerState =>
            {
                if (innerState is (T stateObject, Action<T, CancellationToken> stateAction, CancellationToken stateCancellationToken))
                {
                    stateAction(stateObject, stateCancellationToken);
                }
            }, (state, action, _cancellationToken), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default));
        }

        public void CreateAndEnqueueTask<T>(T state, Func<T, CancellationToken, Task> function)
        {
            _taskToAwait.Add(Task.Factory.StartNew(innerState => innerState is (T stateObject, Func<T, CancellationToken, Task> stateFunction, CancellationToken stateCancellationToken) ? stateFunction(stateObject, stateCancellationToken) : Task.CompletedTask, (state, function, _cancellationToken), _cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).Unwrap());
        }

        public void WaitAll()
        {
            Task.WaitAll(_taskToAwait.ToArray());
        }

        public void Dispose()
        {
            var taskToAwait = _taskToAwait;

            foreach (var task in taskToAwait)
            {
                task.Dispose();
            }
        }
    }
}
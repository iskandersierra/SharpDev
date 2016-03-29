using System;
using System.Threading;
using System.Threading.Tasks;

namespace SharpDev
{
    public class AsyncDisposableValue<T>
    {
        private readonly Func<CancellationToken, Task> _disposeAction;
        private bool _isDisposed;
        private Task disposingTask;

        public AsyncDisposableValue(T value, Func<Task> disposeAction)
            : this(value, _ => disposeAction())
        {
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));
        }

        public AsyncDisposableValue(T value, Func<CancellationToken, Task> disposeAction)
        {
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));
            _disposeAction = disposeAction;
            Value = value;
            _isDisposed = false;
        }

        public T Value { get; }

        public Task DisposeAsync(CancellationToken cancellationToken = default (CancellationToken))
        {
            if (_isDisposed) return disposingTask;
            _isDisposed = true;
            GC.SuppressFinalize(this);
            return disposingTask = _disposeAction(cancellationToken);
        }
    }
}
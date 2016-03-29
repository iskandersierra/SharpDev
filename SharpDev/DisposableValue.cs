using System;

namespace SharpDev
{
    public class DisposableValue<T> : IDisposable
    {
        private readonly Action _disposeAction;
        private bool _isDisposed;

        public DisposableValue(T value, Action disposeAction)
        {
            _disposeAction = disposeAction;
            Value = value;
            _isDisposed = false;
        }

        public T Value { get; }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            GC.SuppressFinalize(this);
            _disposeAction();
        }
    }
}
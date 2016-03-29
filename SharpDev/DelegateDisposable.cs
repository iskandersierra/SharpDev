using System;

namespace SharpDev
{
    public class DelegateDisposable : IDisposable
    {
        private readonly Action _disposeAction;
        private bool _isDisposed;

        public DelegateDisposable(Action disposeAction)
        {
            _disposeAction = disposeAction;
            _isDisposed = false;
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            GC.SuppressFinalize(this);
            _disposeAction();
        }
    }
}

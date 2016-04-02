using System;

namespace SharpDev
{
    public class DelegateDisposable : IDisposable
    {
        private readonly Action _disposeAction;
        private bool _isDisposed;

        public DelegateDisposable(Action startingAction, Action disposeAction)
        {
            if (startingAction == null) throw new ArgumentNullException(nameof(startingAction));
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));

            startingAction();
            _disposeAction = disposeAction;
            _isDisposed = false;
        }

        public DelegateDisposable(Action disposeAction)
        {
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));
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

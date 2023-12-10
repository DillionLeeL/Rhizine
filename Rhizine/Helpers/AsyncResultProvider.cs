using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rhizine.Helpers
{
    public partial class AsyncResultProvider<TResult> : ObservableObject, IResultProvider<TResult>, IDisposable
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public event EventHandler<OperationCompletedEventArgs<TResult>> OperationCompleted;

        [ObservableProperty]
        private TResult _result;

        [ObservableProperty]
        private Exception _error;

        private bool _isCompleted;

        public bool IsCompleted
        {
            get => _isCompleted;
            private set
            {
                if (SetProperty(ref _isCompleted, value))
                {
                    OnOperationCompleted();
                }
            }
        }

        public async Task SetResultAsync(TResult result, CancellationToken cancellationToken)
        {
            await _semaphoreSlim.WaitAsync(cancellationToken);
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    Cancel();
                    return;
                }

                Result = result;
                IsCompleted = true;
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        // async void so it can be used as an event handler
        public async void Cancel()
        {
            await _semaphoreSlim.WaitAsync();
            try
            {
                if (!_isCompleted)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource = new CancellationTokenSource();
                    IsCompleted = true;
                }
            }
            finally
            {
                _semaphoreSlim.Release();
            }
        }

        private void OnOperationCompleted()
        {
            OperationCompleted?.Invoke(this, new OperationCompletedEventArgs<TResult>(_result, _cancellationTokenSource.IsCancellationRequested, _error));
        }

        public void Dispose()
        {
            _cancellationTokenSource.Dispose();
            _semaphoreSlim.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
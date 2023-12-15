using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rhizine.Helpers;

public partial class AsyncResultProvider<TResult> : ObservableObject, IResultProvider<TResult>, IDisposable
{
    private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);
    private CancellationTokenSource _cancellationTokenSource = new();

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

    /// <summary>
    /// Asynchronously sets the result of the operation.
    /// Ensures that the setting of the result is thread-safe and handles cancellation requests.
    /// </summary>
    /// <param name="result">The result to be set.</param>
    /// <param name="cancellationToken">A token to observe for cancellation requests.</param>
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

    /// <summary>
    /// Cancels the ongoing operation.
    /// This method is asynchronous and avoids blocking the caller. It's safe to use as an event handler.
    /// </summary>
    public async void Cancel()
    {
        await _semaphoreSlim.WaitAsync();
        try
        {
            if (!_isCompleted)
            {
                await _cancellationTokenSource.CancelAsync();
                _cancellationTokenSource = new CancellationTokenSource();
                IsCompleted = true;
            }
        }
        finally
        {
            _semaphoreSlim.Release();
        }
    }

    /// <summary>
    /// Invokes the OperationCompleted event with appropriate arguments.
    /// This method is called whenever the IsCompleted property is set to true.
    /// </summary>
    private void OnOperationCompleted()
    {
        OperationCompleted?.Invoke(this, new OperationCompletedEventArgs<TResult>(Result, _cancellationTokenSource.IsCancellationRequested, Error));
    }

    /// <summary>
    /// Disposes of the CancellationTokenSource and SemaphoreSlim to free resources.
    /// Also suppresses finalization to optimize garbage collection.
    /// </summary>
    public void Dispose()
    {
        _cancellationTokenSource.Dispose();
        _semaphoreSlim.Dispose();
        GC.SuppressFinalize(this);
    }
}
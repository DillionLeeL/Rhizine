namespace Rhizine.Core.Helpers;

public interface IResultProvider<TResult>
{
    event EventHandler<OperationCompletedEventArgs<TResult>> OperationCompleted;

    Exception Error { get; }
    bool IsCompleted { get; }
    TResult Result { get; }

    void Cancel();

    Task SetResultAsync(TResult result, CancellationToken cancellationToken);
}

public class OperationCompletedEventArgs<TResult>(TResult result, bool isCancelled, Exception error) : EventArgs
{
    public Exception Error { get; } = error;
    public bool IsCancelled { get; } = isCancelled;
    public TResult Result { get; } = result;
}
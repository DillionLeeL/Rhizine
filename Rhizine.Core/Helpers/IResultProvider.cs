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

public class OperationCompletedEventArgs<TResult> : EventArgs
{
    public OperationCompletedEventArgs(TResult result, bool isCancelled, Exception error)
    {
        Result = result;
        IsCancelled = isCancelled;
        Error = error;
    }

    public Exception Error { get; }
    public bool IsCancelled { get; }
    public TResult Result { get; }
}
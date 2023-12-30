namespace Rhizine.Services.Interfaces
{
    public interface ILoggingService
    {
        void LogDebug(string message);

        Task LogDebugAsync(string message) => Task.Run(() => LogDebug(message));

        void Log(string message) => LogInformation(message);

        Task LogAsync(string message) => LogInformationAsync(message);

        void LogInformation(string message);

        Task LogInformationAsync(string message) => Task.Run(() => LogInformation(message));

        void LogWarning(string message);

        Task LogWarningAsync(string message) => Task.Run(() => LogWarning(message));

        void LogError(string message);

        Task LogErrorAsync(string message) => Task.Run(() => LogError(message));

        void Log(Exception exception, string message) => LogError(exception, message);

        Task LogAsync(Exception exception, string message) => LogErrorAsync(exception, message);

        void LogError(Exception exception, string message);

        Task LogErrorAsync(Exception exception, string message) => Task.Run(() => LogError(exception, message));

        void Log(Exception exception) => LogError(exception);

        Task LogAsync(Exception exception) => LogErrorAsync(exception);

        void LogError(Exception exception);

        Task LogErrorAsync(Exception exception) => Task.Run(() => LogError(exception));

        // Global exception handler for uncaught exceptions
        void HandleGlobalException(Exception exception);
    }
}
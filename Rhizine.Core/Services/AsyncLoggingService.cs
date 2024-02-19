using Microsoft.Extensions.Logging;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using System.Diagnostics;

// Just an example, async logging through providers like serilog
// are much more efficient and should be used instead.
namespace Rhizine.Core.Services
{
    public class AsyncLoggingService(ILogger<AsyncLoggingService> logger) : ILoggingService
    {
        private readonly ILogger<AsyncLoggingService> _logger = logger;

        public async Task LogDebugAsync(string message)
        {
            await Task.Run(() => LoggingMessages.LogDebug(_logger, message));
        }

        public Task LogAsync(string message) => LogInformationAsync(message);

        public async Task LogInformationAsync(string message)
        {
            await Task.Run(() => LoggingMessages.LogInformation(_logger, message));
        }

        public async Task LogWarningAsync(string message)
        {
            await Task.Run(() => LoggingMessages.LogWarning(_logger, message));
        }

        public async Task LogErrorAsync(string message)
        {
            await Task.Run(() => LoggingMessages.LogError(_logger, message));
        }

        public Task LogAsync(Exception exception, string message) => LogErrorAsync(exception, message);

        public async Task LogErrorAsync(Exception exception, string message)
        {
            await Task.Run(() => LoggingMessages.LogException(_logger, exception, message));
        }

        public Task LogAsync(Exception exception) => LogErrorAsync(exception);

        public async Task LogErrorAsync(Exception exception)
        {
            await Task.Run(() => LoggingMessages.LogException(_logger, exception));
        }

        public async Task LogPerformanceAsync(Action action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                stopwatch.Stop();
                await Task.Run(() => LoggingMessages.LogPerformance(_logger, actionName, stopwatch.ElapsedMilliseconds));
            }
        }

        /// <summary>
        /// Logs a debug message.
        /// </summary>
        /// <param name="message">The debug message to log.</param>
        public void LogDebug(string message)
        {
            LoggingMessages.LogDebug(_logger, message);
        }

        /// <summary>
        /// Logs an informational message. This is the default log method.
        /// </summary>
        /// <param name="message">The message to log.</param>
        public void Log(string message) => LogInformation(message);

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="message">The informational message to log.</param>
        public void LogInformation(string message)
        {
            LoggingMessages.LogInformation(_logger, message);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="message">The warning message to log.</param>
        public void LogWarning(string message)
        {
            LoggingMessages.LogWarning(_logger, message);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="message">The error message to log.</param>
        public void LogError(string message)
        {
            LoggingMessages.LogError(_logger, message);
        }

        /// <summary>
        /// Logs an error message along with an associated exception.
        /// </summary>
        /// <param name="exception">The exception related to the error.</param>
        /// <param name="message">The error message to log.</param>
        public void Log(Exception exception, string message) => LogError(exception, message);

        /// <summary>
        /// Logs an error message along with an associated exception.
        /// </summary>
        /// <param name="exception">The exception related to the error.</param>
        /// <param name="message">The error message to log.</param>
        public void LogError(Exception exception, string message)
        {
            LoggingMessages.LogException(_logger, exception, message);
        }

        /// <summary>
        /// Logs an exception as an error.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public void Log(Exception exception) => LogError(exception);

        /// <summary>
        /// Logs an exception as an error.
        /// </summary>
        /// <param name="exception">The exception to log.</param>
        public void LogError(Exception exception)
        {
            LoggingMessages.LogException(_logger, exception);
        }

        /// <summary>
        /// Logs the duration of a given action for performance monitoring.
        /// </summary>
        /// <param name="action">The action to monitor.</param>
        /// <param name="actionName">The name of the action being monitored.</param>
        public void LogPerformance(Action action, string actionName)
        {
            var stopwatch = Stopwatch.StartNew();
            try
            {
                action();
            }
            finally
            {
                stopwatch.Stop();
                LoggingMessages.LogPerformance(_logger, actionName, stopwatch.ElapsedMilliseconds);
            }
        }

        /// <summary>
        /// Handles and logs global exceptions.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        public void HandleGlobalException(Exception exception)
        {
            LogError(exception, "An unexpected error has occured.");
        }
    }
}
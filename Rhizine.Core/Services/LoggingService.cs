using Microsoft.Extensions.Logging;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using System.Diagnostics;

namespace Rhizine.Core.Services;

// Frameworks like Serilog can be configured to log asynchonously under the hood
// but the actual calls to the logger are still synchronous, which is why I have
// Serilog configured as a LoggingService and not an AsyncLoggingService

// TODO: aggregation -> ELK (Elasticsearch, Logstash, Kibana
// TODO: Correlation IDs

/// <summary>
/// Initializes a new instance of the LoggingService class.
/// </summary>
/// <param name="logger">The logger instance used for logging operations.</param>
public class LoggingService(ILogger<LoggingService> logger) : ILoggingService
{
    private readonly ILogger<LoggingService> _logger = logger;

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
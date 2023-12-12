using Microsoft.Extensions.Logging;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Diagnostics;
using System.Windows;

namespace Rhizine.Services;

// Frameworks like Serilog can be configured to log asynchonously under the hood
// but the actual calls to the logger are still synchronous, which is why I have
// Serilog configured as a LoggingService and not an AsyncLoggingService
public class LoggingService : ILoggingService
{
    // TODO: aggregation -> ELK (Elasticsearch, Logstash, Kibana
    // TODO: Correlation IDs

    private readonly ILogger<LoggingService> _logger;

    public LoggingService(ILogger<LoggingService> logger)
    {
        _logger = logger;
    }

    public void LogDebug(string message)
    {
        LoggingMessages.LogDebug(_logger, message);
    }

    public void Log(string message) => LogInformation(message);

    public void LogInformation(string message)
    {
        LoggingMessages.LogInformation(_logger, message);
    }

    public void LogWarning(string message)
    {
        LoggingMessages.LogWarning(_logger, message);
    }

    public void LogError(string message)
    {
        LoggingMessages.LogError(_logger, message);
    }

    public void Log(Exception exception, string message) => LogError(exception, message);

    public void LogError(Exception exception, string message)
    {
        LoggingMessages.LogException(_logger, exception, message);
    }

    public void Log(Exception exception) => LogError(exception);

    public void LogError(Exception exception)
    {
        LoggingMessages.LogException(_logger, exception);
    }

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

    public void HandleGlobalException(Exception exception)
    {
        LogError(exception, "An unexpected error has occured.");
        MessageBox.Show(exception.Message);
    }
}
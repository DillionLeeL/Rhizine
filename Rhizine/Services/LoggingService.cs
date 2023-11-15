using Microsoft.Extensions.Logging;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Diagnostics;

// TODO: Structured logging
namespace Rhizine.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogDebug(string message)
        {
            LoggingMessages.LogDebug(_logger, message);
        }
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

        public void LogError(Exception exception, string message)
        {
            LoggingMessages.LogException(_logger, exception, message);
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

    }
}

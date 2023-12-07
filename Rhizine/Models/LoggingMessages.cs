using Microsoft.Extensions.Logging;

namespace Rhizine.Models
{
    public static partial class LoggingMessages
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Debug, Message = "{Message}")]
        public static partial void LogDebug(ILogger logger, string message);

        [LoggerMessage(EventId = 2, Level = LogLevel.Information, Message = "{Message}")]
        public static partial void LogInformation(ILogger logger, string message);

        [LoggerMessage(EventId = 3, Level = LogLevel.Warning, Message = "{Message}")]
        public static partial void LogWarning(ILogger logger, string message);

        [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "{Message}")]
        public static partial void LogError(ILogger logger, string message);

        [LoggerMessage(EventId = 5, Level = LogLevel.Error, SkipEnabledCheck = true)]
        public static partial void LogException(ILogger logger, Exception exception);

        [LoggerMessage(EventId = 6, Level = LogLevel.Error, Message = "{Message}", SkipEnabledCheck = true)]
        public static partial void LogException(ILogger logger, Exception exception, string message);

        [LoggerMessage(EventId = 10, Level = LogLevel.Debug, Message = "Action {ActionName} completed in {ElapsedMilliseconds} ms")]
        public static partial void LogPerformance(ILogger logger, string actionName, long elapsedMilliseconds);
        
    }
}
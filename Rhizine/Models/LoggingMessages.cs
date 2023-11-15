using Microsoft.Extensions.Logging;

namespace Rhizine.Models
{
    public static partial class LoggingMessages
    {
        [LoggerMessage(EventId = 1, Level = LogLevel.Information, Message = "{Message}")]
        public static partial void LogInformation(ILogger logger, string message);

        [LoggerMessage(EventId = 2, Level = LogLevel.Warning, Message = "{Message}")]
        public static partial void LogWarning(ILogger logger, string message);

        [LoggerMessage(EventId = 3, Level = LogLevel.Error, Message = "{Message}")]
        public static partial void LogError(ILogger logger, string message);

        [LoggerMessage(EventId = 4, Level = LogLevel.Error, Message = "{Message}")]
        public static partial void LogException(ILogger logger, Exception exception, string message);


    }
}

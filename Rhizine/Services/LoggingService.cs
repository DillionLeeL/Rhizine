using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhizine.Services.Interfaces;

namespace Rhizine.Services
{
    public class LoggingService : ILoggingService
    {
        private readonly ILogger<LoggingService> _logger;

        public LoggingService(ILogger<LoggingService> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation("{Message}", message);
        }

        public void LogWarning(string message)
        {
            _logger.LogWarning("{Message}", message);
        }

        public void LogError(string message)
        {
            _logger.LogError("{Message}", message);
        }

        public void logError(Exception exception)
        {
            _logger.LogError("{Error}", exception);
        }
        public void LogError(Exception exception, string message)
        {
            _logger.LogError(exception, message);
        }
        public void LogInformation(string message, params object[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogError(string message, params object[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception exception, string message, params object[] args)
        {
            _logger.LogError(exception, message, args);
        }
    }
}

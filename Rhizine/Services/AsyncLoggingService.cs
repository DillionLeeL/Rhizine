using Microsoft.Extensions.Logging;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Diagnostics;
using System.Windows;

namespace Rhizine.Services
{
    public class AsyncLoggingService : ILoggingService
    {
        private readonly ILogger<AsyncLoggingService> _logger;

        public AsyncLoggingService(ILogger<AsyncLoggingService> logger)
        {
            _logger = logger;
        }

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

        public void HandleGlobalException(Exception exception)
        {
            LogError(exception, "An unexpected error has occured.");
            MessageBox.Show(exception.Message);
        }

        public void LogDebug(string message)
        {
            throw new NotImplementedException();
        }

        public void LogInformation(string message)
        {
            throw new NotImplementedException();
        }

        public void LogWarning(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception exception, string message)
        {
            throw new NotImplementedException();
        }

        public void LogError(Exception exception)
        {
            throw new NotImplementedException();
        }
    }
}
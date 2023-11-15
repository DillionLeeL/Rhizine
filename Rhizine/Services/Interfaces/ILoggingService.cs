using Microsoft.Extensions.Logging;

namespace Rhizine.Services.Interfaces
{
    public interface ILoggingService
    {
        void LogDebug(string message);
        void LogInformation(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogError(Exception exception, string message);
    }
}

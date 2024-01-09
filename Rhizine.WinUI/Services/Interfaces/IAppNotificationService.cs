using System.Collections.Specialized;

namespace Rhizine.WinUI.Services.Interfaces;

public interface IAppNotificationService
{
    void Initialize();

    bool Show(string payload);

    NameValueCollection ParseArguments(string arguments);

    void Unregister();
}
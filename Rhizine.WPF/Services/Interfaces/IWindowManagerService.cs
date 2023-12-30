using System.Windows;

namespace Rhizine.WPF.Services.Interfaces;

public interface IWindowManagerService
{
    Window MainWindow { get; }

    void OpenInNewWindow(string pageKey, object parameter = null);

    bool? OpenInDialog(string pageKey, object parameter = null);

    Window GetWindow(string pageKey);
}

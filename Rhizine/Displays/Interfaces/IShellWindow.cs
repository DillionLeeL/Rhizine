using System.Windows.Controls;

namespace Rhizine.Displays.Interfaces;

public interface IShellWindow
{
    Frame GetNavigationFrame();
    void ShowWindow();
    void CloseWindow();
}

using MahApps.Metro.Controls;
using System.Windows.Controls;

namespace Rhizine.Displays.Interfaces;

public interface IShellWindow
{
    Frame GetNavigationFrame();
    //FlyoutsControl GetFlyoutsControl();
    void ShowWindow();

    void CloseWindow();
}

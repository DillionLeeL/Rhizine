using System.Windows.Controls;

namespace Rhizine.WPF.Views.Interfaces;

public interface IShellWindow
{
    Frame GetNavigationFrame();

    void ShowWindow();

    void CloseWindow();
}
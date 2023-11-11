using System.Windows.Controls;

using MahApps.Metro.Controls;
using Rhizine.Displays.Interfaces;
using Rhizine.Displays.Windows;

namespace Rhizine.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame()
        => shellFrame;

    public void ShowWindow()
        => Show();

    public void CloseWindow()
        => Close();
}

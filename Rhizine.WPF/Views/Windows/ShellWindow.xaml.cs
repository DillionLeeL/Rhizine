using MahApps.Metro.Controls;
using Rhizine.Displays.Windows;
using Rhizine.WPF.Views.Interfaces;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Windows;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame() => shellFrame;

    public void ShowWindow() => Show();

    public void CloseWindow() => Close();
}
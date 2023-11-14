using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Rhizine.Displays.Interfaces;
using Rhizine.Displays.Windows;
using Rhizine.Services;

namespace Rhizine.Views;

public partial class ShellWindow : MetroWindow, IShellWindow
{
    public ShellWindow(ShellViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    public Frame GetNavigationFrame() => shellFrame;
    //public FlyoutsControl GetFlyoutsControl() => flyoutsControl;
    public void ShowWindow() => Show();

    public void CloseWindow() => Close();
}

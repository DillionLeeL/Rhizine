using System.Windows.Controls;

using MahApps.Metro.Controls;
using Rhizine.WPF.ViewModels;
using Rhizine.WPF.Views.Interfaces;

namespace Rhizine.WPF.Views.Windows;

public partial class ShellDialogWindow : MetroWindow, IShellDialogWindow
{
    public ShellDialogWindow(ShellDialogViewModel viewModel)
    {
        InitializeComponent();
        viewModel.SetResult = OnSetResult;
        DataContext = viewModel;
    }

    public Frame GetDialogFrame()
        => dialogFrame;

    private void OnSetResult(bool? result)
    {
        DialogResult = result;
        Close();
    }
}

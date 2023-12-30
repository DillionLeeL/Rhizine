using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using Rhizine.Core.ViewModels;

namespace Rhizine.WPF.ViewModels.Flyouts;

public partial class FlyoutBaseViewModel : BaseViewModel
{
    [ObservableProperty]
    private string _header;

    [ObservableProperty]
    private bool _isOpen;

    [ObservableProperty]
    private Position _position;

    [ObservableProperty]
    private FlyoutTheme _theme = FlyoutTheme.Dark;

    [ObservableProperty]
    private bool _isModal;

    [RelayCommand]
    private void Close()
    {
        IsOpen = false;
    }
}
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using MahApps.Metro.Controls;
using Rhizine.Displays.Flyouts;
using Rhizine.Displays.Pages;
using Rhizine.Models;
using Rhizine.Properties;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using WPFBase.Displays;

namespace Rhizine.Displays.Windows;

public partial class ShellViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ILoggingService _loggingService;
    [ObservableProperty]
    private IFlyoutService _flyoutService;
    [ObservableProperty]
    private HamburgerMenuItem _selectedMenuItem;
    [ObservableProperty]
    private HamburgerMenuItem _selectedOptionsMenuItem;

    // TODO: Change the icons and titles for all HamburgerMenuItems here.
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellLandingPage, Glyph = "\uE8A5", TargetPageType = typeof(LandingViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellWebViewPage, Glyph = "\uE8A5", TargetPageType = typeof(WebViewViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(DataGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellContentGridPage, Glyph = "\uE8A5", TargetPageType = typeof(ContentGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellListDetailsPage, Glyph = "\uE8A5", TargetPageType = typeof(ListDetailsViewModel) },
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new ObservableCollection<HamburgerMenuItem>()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public ShellViewModel(INavigationService navigationService, IFlyoutService flyoutService, ILoggingService loggingService)
    {
        _navigationService = navigationService;

        FlyoutService = flyoutService;
        _loggingService = loggingService;
        _loggingService.LogInformation("shell constructed");
        _flyoutService.OnFlyoutOpened += FlyoutOpened;
        _flyoutService.OnFlyoutClosed += FlyoutClosed;
    }
    private bool CanGoBack() => _navigationService.CanGoBack;

    [RelayCommand]
    private void OnLoaded()
    {
        _navigationService.Navigated += OnNavigated;
    }
    [RelayCommand]
    private void OnUnloaded()
    {
        _navigationService.Navigated -= OnNavigated;
    }
    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void GoBack() => _navigationService.GoBack();

    [RelayCommand]
    private void OnMenuItemInvoked() => NavigateTo(SelectedMenuItem.TargetPageType);

    [RelayCommand]
    private void OnOptionsMenuItemInvoked() => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateTo(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, string viewModelName)
    {
        var item = MenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        if (item != null)
        {
            SelectedMenuItem = item;
        }
        else
        {
            SelectedOptionsMenuItem = OptionMenuItems
                    .OfType<HamburgerMenuItem>()
                    .FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
        }

        GoBackCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void FlyoutOpened(string flyout)
    {
        _loggingService.LogInformation("FlyoutOpened");
    }
    [RelayCommand]
    private void FlyoutClosed(string flyout)
    {
        _loggingService.LogInformation("FlyoutOpened");
    }
}

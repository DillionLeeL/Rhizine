using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.Displays.Pages;
using Rhizine.WPF.Helpers;
using Rhizine.WPF.Properties;
using Rhizine.WPF.Services.Interfaces;
using Rhizine.WPF.ViewModels.Pages;
using System.Collections.ObjectModel;
using System.Windows.Navigation;

namespace Rhizine.Displays.Windows;

public partial class ShellViewModel : BaseViewModel
{
    private readonly INavigationService<NavigationEventArgs> _navigationService;
    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private IFlyoutService _flyoutService;

    [ObservableProperty]
    private HamburgerMenuItem _selectedMenuItem;

    [ObservableProperty]
    private HamburgerMenuItem _selectedOptionsMenuItem;

    // TODO: Change the icons and titles for all HamburgerMenuItems here
    public ObservableCollection<HamburgerMenuItem> MenuItems { get; } = new()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellLandingPage, Glyph = "\uE8A5", TargetPageType = typeof(LandingViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellWebViewPage, Glyph = "\uE8A5", TargetPageType = typeof(WebViewViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellDataGridPage, Glyph = "\uE8A5", TargetPageType = typeof(DataGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellContentGridPage, Glyph = "\uE8A5", TargetPageType = typeof(ContentGridViewModel) },
        new HamburgerMenuGlyphItem() { Label = Resources.ShellListDetailsPage, Glyph = "\uE8A5", TargetPageType = typeof(ListDetailsViewModel) },
    };

    public ObservableCollection<HamburgerMenuItem> OptionMenuItems { get; } = new()
    {
        new HamburgerMenuGlyphItem() { Label = Resources.ShellSettingsPage, Glyph = "\uE713", TargetPageType = typeof(SettingsViewModel) }
    };

    public ShellViewModel(INavigationService<NavigationEventArgs> navigationService, IFlyoutService flyoutService, ILoggingService loggingService)
    {
        _navigationService = navigationService;

        FlyoutService = flyoutService;
        _loggingService = loggingService;
        if (_flyoutService != null)
        {
            _flyoutService.OnFlyoutOpened += FlyoutOpened;
            _flyoutService.OnFlyoutClosed += FlyoutClosed;
        }
    }

    public Func<HamburgerMenuItem, bool> IsPageRestricted { get; } =
    (menuItem) => Attribute.IsDefined(menuItem.TargetPageType, typeof(Restricted));

    //private bool CanGoBack => _navigationService.CanGoBack;

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

    [RelayCommand] // (CanExecute = nameof(CanGoBack))
    private void GoBack() => _navigationService.GoBackAsync();

    [RelayCommand]
    private void OnMenuItemInvoked() => NavigateTo(SelectedMenuItem.TargetPageType);

    [RelayCommand]
    private void OnOptionsMenuItemInvoked() => NavigateTo(SelectedOptionsMenuItem.TargetPageType);

    private void NavigateTo(Type targetViewModel)
    {
        if (targetViewModel != null)
        {
            _navigationService.NavigateToAsync(targetViewModel.FullName);
        }
    }

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        string viewModelName = e?.Content?.GetType()?.FullName;

        if (!string.IsNullOrEmpty(viewModelName))
        {
            var menuItem = FindMenuItem(MenuItems, viewModelName) ?? FindMenuItem(OptionMenuItems, viewModelName);

            if (menuItem != null)
            {
                SelectedMenuItem = menuItem;
                SelectedOptionsMenuItem = OptionMenuItems.Contains(menuItem) ? menuItem as HamburgerMenuItem : null;

                GoBackCommand.NotifyCanExecuteChanged();
            }
        }
    }

    private static HamburgerMenuItem FindMenuItem(IEnumerable<HamburgerMenuItem> items, string viewModelName)
    {
        return items?.FirstOrDefault(i => viewModelName == i.TargetPageType?.FullName);
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
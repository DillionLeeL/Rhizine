using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using Microsoft.Extensions.Options;
using Rhizine.Displays.Flyouts;
using Rhizine.Displays.Interfaces;
using Rhizine.Models;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

// TODO: Change the URL for your privacy policy in the appsettings.json file, currently set to https://YourPrivacyUrlGoesHere
public partial class SettingsViewModel : BaseViewModel
{
    private readonly AppConfig _appConfig;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly ISystemService _systemService;
    private readonly IApplicationInfoService _applicationInfoService;
    private readonly ILoggingService _loggingService;
    [ObservableProperty]
    private AppTheme _theme;
    [ObservableProperty]
    private string _versionDescription;
    [ObservableProperty]
    private FlyoutsControl _flyoutsControl;
    [ObservableProperty]
    public Flyout testFlyout;
    public SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService, ISystemService systemService, 
                             IApplicationInfoService applicationInfoService, ILoggingService loggingService)
    {
        _appConfig = appConfig.Value;
        _themeSelectorService = themeSelectorService;
        _systemService = systemService;
        _applicationInfoService = applicationInfoService;
        _loggingService = loggingService;
        //this.OpenFlyoutCommand = new SimpleCommand<Flyout>(f => f is not null, f => f!.SetCurrentValue(Flyout.IsOpenProperty, true));
        //this.CloseFlyoutCommand = new SimpleCommand<Flyout>(f => f is not null, f => f!.SetCurrentValue(Flyout.IsOpenProperty, false));
    }

    public void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {_applicationInfoService.GetVersion()}";
        Theme = _themeSelectorService.GetCurrentTheme();
    }

    public void OnNavigatedFrom()
    {
    }
    [RelayCommand]
    private void SetTheme(string themeName)
    {
        var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
        _themeSelectorService.SetTheme(theme);
    }
    [RelayCommand]
    private void PrivacyStatement() => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);



    /*
    public ICommand OpenFlyoutCommand { get; }

    public ICommand CloseFlyoutCommand { get; }
    private void ShowDynamicFlyout(object sender, RoutedEventArgs e)
    {
        var flyout = new DynamicFlyout
        {
            Header = "Dynamic flyout"
        };

        // when the flyout is closed, remove it from the hosting FlyoutsControl
        void ClosingFinishedHandler(object o, RoutedEventArgs args)
        {
            flyout.ClosingFinished -= ClosingFinishedHandler;
            this.flyoutsControl.Items.Remove(flyout);
        }

        flyout.ClosingFinished += ClosingFinishedHandler;

        this.flyoutsControl.Items.Add(flyout);

        flyout.IsOpen = true;
    }
    */
}

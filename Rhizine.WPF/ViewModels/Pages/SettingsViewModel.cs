using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using Microsoft.Extensions.Options;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.WPF.Models;
using Rhizine.WPF.Properties;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly AppConfig _appConfig;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IApplicationInfoService _applicationInfoService;
    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private AppTheme _theme;

    [ObservableProperty]
    private string _versionDescription;

    [ObservableProperty]
    private FlyoutsControl _flyoutsControl;

    [ObservableProperty]
    private Flyout _testFlyout;

    public SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService,
                             IApplicationInfoService applicationInfoService, ILoggingService loggingService)
    {
        _appConfig = appConfig?.Value ?? new AppConfig();
        _themeSelectorService = themeSelectorService;
        _applicationInfoService = applicationInfoService;
        _loggingService = loggingService;
    }

    public override void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Resources.AppDisplayName} - {_applicationInfoService.GetVersion()}";
        Theme = _themeSelectorService.GetCurrentTheme();
    }

    [RelayCommand]
    private void SetTheme(string themeName)
    {
        var theme = (AppTheme)Enum.Parse(typeof(AppTheme), themeName);
        _themeSelectorService.SetTheme(theme);
    }

    //[RelayCommand]
    //private void PrivacyStatement() => _systemService.OpenInWebBrowser(_appConfig.PrivacyStatement);
}
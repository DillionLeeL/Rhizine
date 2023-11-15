using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls;
using Microsoft.Extensions.Options;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

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
    }

    public override void OnNavigatedTo(object parameter)
    {
        VersionDescription = $"{Properties.Resources.AppDisplayName} - {_applicationInfoService.GetVersion()}";
        Theme = _themeSelectorService.GetCurrentTheme();
    }

    public override void OnNavigatedFrom()
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
}

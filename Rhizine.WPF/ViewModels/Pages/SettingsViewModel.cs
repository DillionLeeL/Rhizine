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

public partial class SettingsViewModel(IOptions<AppConfig> appConfig, IThemeSelectorService themeSelectorService,
                         IApplicationInfoService applicationInfoService, ILoggingService loggingService) : BaseViewModel
{
    #region Fields

    private readonly AppConfig _appConfig = appConfig?.Value ?? new AppConfig();
    private readonly IApplicationInfoService _applicationInfoService = applicationInfoService;
    private readonly ILoggingService _loggingService = loggingService;
    private readonly IThemeSelectorService _themeSelectorService = themeSelectorService;
    [ObservableProperty]
    private FlyoutsControl _flyoutsControl;

    [ObservableProperty]
    private Flyout _testFlyout;

    [ObservableProperty]
    private AppTheme _theme;

    [ObservableProperty]
    private string _versionDescription;

    #endregion Fields

    #region Methods

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

    #endregion Methods

}
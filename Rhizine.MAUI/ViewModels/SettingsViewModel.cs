using Rhizine.Core.Services.Interfaces;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;
using Microsoft.Maui.Controls;
using AppTheme = Rhizine.Core.Models.AppTheme;
namespace Rhizine.MAUI.ViewModels;

public partial class SettingsViewModel : BaseViewModel
{
    private readonly IThemeSelectorService _themeSelectorService;

    public IEnumerable<AppTheme> AppThemes => Enum.GetValues(typeof(AppTheme)).Cast<AppTheme>();

    public AppTheme CurrentAppTheme => _themeSelectorService.GetCurrentTheme();

    [ObservableProperty]
    private string appVersion;

    [ObservableProperty]
    private AppTheme themeSelection;

    public SettingsViewModel(IThemeSelectorService themeService, IDialogService dialogService, INavigationService navigationService)
            : base(dialogService, navigationService)
    {
        _themeSelectorService = themeService;
        AppVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
        ThemeSelection = _themeSelectorService.GetCurrentTheme();
    }

    [RelayCommand]
    private void ChangeTheme(AppTheme theme)
    {
        if (theme == _themeSelectorService.GetCurrentTheme())
            return;

        _themeSelectorService.SetTheme(theme);
    }
}

using ControlzEx.Theming;
using MahApps.Metro.Theming;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Windows;

namespace Rhizine.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string HcDarkTheme = "pack://application:,,,/Styles/Themes/HC.Dark.Teal.xaml";
    private const string HcLightTheme = "pack://application:,,,/Styles/Themes/HC.Light.Teal.xaml";

    public ThemeSelectorService()
    {
    }

    public void InitializeTheme()
    {
        // https://mahapps.com/docs/themes/thememanager#creating-custom-themes
        ThemeManager.Current.AddLibraryTheme(new LibraryTheme(new Uri(HcDarkTheme), MahAppsLibraryThemeProvider.DefaultInstance));
        ThemeManager.Current.AddLibraryTheme(new LibraryTheme(new Uri(HcLightTheme), MahAppsLibraryThemeProvider.DefaultInstance));

        var theme = GetCurrentTheme();
        SetTheme(theme);
    }

    public void SetTheme(AppTheme theme)
    {
        if (theme == AppTheme.Default)
        {
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();
        }
        else
        {
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithHighContrast;
            ThemeManager.Current.SyncTheme();
            ThemeManager.Current.ChangeTheme(Application.Current, $"{theme}.Teal", SystemParameters.HighContrast);
        }

        Application.Current.Properties["Theme"] = theme.ToString();
    }

    public AppTheme GetCurrentTheme()
    {
        if (Application.Current.Properties.Contains("Theme"))
        {
            var themeName = Application.Current.Properties["Theme"]?.ToString();
            return Enum.TryParse(themeName, out AppTheme theme) ? theme : AppTheme.Default;
        }

        return AppTheme.Default;
    }
}
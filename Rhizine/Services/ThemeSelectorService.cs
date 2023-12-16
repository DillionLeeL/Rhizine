using ControlzEx.Theming;
using MahApps.Metro.Theming;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using Serilog.Core;
using System.Windows;
using System.Windows.Media;

namespace Rhizine.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string HcDarkTheme = "pack://application:,,,/Styles/Themes/HC.Dark.Teal.xaml";
    private const string HcLightTheme = "pack://application:,,,/Styles/Themes/HC.Light.Teal.xaml";
    private const string HcDarkGrayTheme = "pack://application:,,,/Styles/Themes/Dark.Gray.xaml";
    private readonly Dictionary<AppTheme, Theme> _customThemes = new();

    public ThemeSelectorService() { }

    public void InitializeTheme()
    {
        // https://mahapps.com/docs/themes/thememanager#creating-custom-themes
        _customThemes.Add(AppTheme.Dark, ThemeManager.Current.AddLibraryTheme(new LibraryTheme(new Uri(HcDarkGrayTheme), MahAppsLibraryThemeProvider.DefaultInstance)));

        var theme = GetCurrentTheme();
        SetTheme(theme);
    }

    public void SetTheme(AppTheme theme)
    {
        if (_customThemes.TryGetValue(theme, out Theme customTheme))
        {
            if (customTheme.IsHighContrast)
            {
                ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncWithHighContrast;
                ThemeManager.Current.SyncTheme();
            }
            ThemeManager.Current.ChangeTheme(Application.Current, customTheme.Name, customTheme.IsHighContrast);
        }
        else
        {
            string brightness = theme == AppTheme.Light ? "Light" : "Dark";
            string themeName = $"{brightness}.Teal";
            ThemeManager.Current.ChangeTheme(Application.Current, themeName);
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
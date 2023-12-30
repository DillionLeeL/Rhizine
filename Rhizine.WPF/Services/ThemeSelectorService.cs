using ControlzEx.Theming;
using MahApps.Metro.Theming;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using System.Windows;
using Windows.System;

namespace Rhizine.WPF.Services;

// https://mahapps.com/docs/themes/thememanager#creating-custom-themes

public class ThemeSelectorService : IThemeSelectorService
{
    private const string HcDarkTheme = "pack://application:,,,/Styles/Themes/HC.Dark.Teal.xaml";
    private const string HcLightTheme = "pack://application:,,,/Styles/Themes/HC.Light.Teal.xaml";
    private const string HcDarkGrayTheme = "pack://application:,,,/Styles/Themes/Dark.Gray.xaml";
    private readonly Dictionary<AppTheme, Theme> _customThemes = new();

    public ThemeSelectorService() { }

    public void Initialize()
    {
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

    // TODO
    public async Task InitializeAsync()
    {
        await Task.Run(() => Initialize()).ConfigureAwait(true);
    }

    public async Task SetThemeAsync(AppTheme theme)
    {
       await Task.Run(() => SetTheme(theme)).ConfigureAwait(true);
    }

    // TODO: loading themes from settings
    /*
    private async Task<AppTheme> LoadThemeFromSettingsAsync()
    {
        var themeName = await _localSettingsService.ReadSettingAsync<string>(SettingsKey);

        if (Enum.TryParse(themeName, out AppTheme cacheTheme))
        {
            return cacheTheme;
        }

        return AppTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(AppTheme theme)
    {
        await _localSettingsService.SaveSettingAsync(SettingsKey, theme.ToString());
    }
    */
}
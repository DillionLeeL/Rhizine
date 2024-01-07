using Microsoft.UI.Xaml;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WinUI.Helpers;

namespace Rhizine.WinUI.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private const string SettingsKey = "AppBackgroundRequestedTheme";

    public AppTheme Theme { get; set; } = AppTheme.Default;

    private readonly ILocalSettingsService _localSettingsService;

    public ThemeSelectorService(ILocalSettingsService localSettingsService)
    {
        _localSettingsService = localSettingsService;
    }

    public async Task InitializeAsync()
    {
        Theme = await LoadThemeFromSettingsAsync();
        await SetRequestedThemeAsync();
    }

    public async Task SetThemeAsync(AppTheme theme)
    {
        Theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(Theme);
    }

    public async Task SetRequestedThemeAsync()
    {
        if (App.MainWindow.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = (ElementTheme)Theme;

            TitleBarHelper.UpdateTitleBar((ElementTheme)Theme);
        }

        await Task.CompletedTask;
    }

    public AppTheme GetCurrentTheme()
    {
        return Theme;
    }

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
}
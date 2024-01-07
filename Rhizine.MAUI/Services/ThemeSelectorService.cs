using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using AppTheme = Rhizine.Core.Models.AppTheme;
using Microsoft.Maui.ApplicationModel;

namespace Rhizine.MAUI.Services;

public class ThemeSelectorService : IThemeSelectorService
{
    private AppTheme _theme;

    public async Task InitializeAsync()
    {
        _theme = await LoadThemeFromSettingsAsync();
        await SetRequestedThemeAsync();
    }

    public async Task SetThemeAsync(AppTheme theme)
    {
        _theme = theme;

        await SetRequestedThemeAsync();
        await SaveThemeInSettingsAsync(_theme);
    }

    public void SetTheme(AppTheme theme)
    {
        if (Application.Current is null)
        {
            throw new NullReferenceException("Application.Current is null");
        }

        _theme = theme;

        Application.Current!.UserAppTheme = theme switch
        {
            AppTheme.Dark => Microsoft.Maui.ApplicationModel.AppTheme.Dark,
            AppTheme.Light => Microsoft.Maui.ApplicationModel.AppTheme.Light,
            _ => Microsoft.Maui.ApplicationModel.AppTheme.Unspecified,
        };
    }

    public async Task SetRequestedThemeAsync()
    {
        // TODO
        await Task.CompletedTask;
    }

    public AppTheme GetCurrentTheme()
    {
        // TODO
        return _theme;
    }

    private async Task<AppTheme> LoadThemeFromSettingsAsync()
    {
        //TODO
        return AppTheme.Default;
    }

    private async Task SaveThemeInSettingsAsync(AppTheme theme)
    {
        // TODO
        await Task.CompletedTask;
    }
}
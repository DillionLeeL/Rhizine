using Rhizine.Core.Models;

namespace Rhizine.Core.Services.Interfaces;

public interface IThemeSelectorService
{
    void Initialize() => InitializeAsync().ConfigureAwait(false);

    Task InitializeAsync();

    void SetTheme(AppTheme theme) => SetThemeAsync(theme).ConfigureAwait(false);

    Task SetThemeAsync(AppTheme theme);

    AppTheme GetCurrentTheme();

    //Task<AppTheme> LoadThemeFromSettingsAsync();
    //Task SaveThemeInSettingsAsync(AppTheme theme);
}
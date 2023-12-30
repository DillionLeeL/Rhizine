using Rhizine.Models;

namespace Rhizine.Services.Interfaces;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
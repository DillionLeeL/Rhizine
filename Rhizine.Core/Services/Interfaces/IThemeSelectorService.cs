using Rhizine.Core.Models;

namespace Rhizine.Core.Services.Interfaces;

public interface IThemeSelectorService
{
    void InitializeTheme();

    void SetTheme(AppTheme theme);

    AppTheme GetCurrentTheme();
}
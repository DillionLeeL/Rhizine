namespace Rhizine.Core.Services.Interfaces;

public interface INavigationService<TNavigationSource, TEventArgs>
{
    TNavigationSource NavigationSource { get; }

    event EventHandler<TEventArgs> Navigated;

    bool CanGoBack { get; }

    void Initialize(TNavigationSource source);

    bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false);

    bool GoBack();

    Task NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false);

    Task GoBackAsync();
}
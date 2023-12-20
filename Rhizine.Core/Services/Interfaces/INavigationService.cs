namespace Rhizine.Core.Services.Interfaces;

public interface INavigationService<TEventArgs> where TEventArgs : EventArgs
{
    event EventHandler<TEventArgs> Navigated;

    void Initialize(object? parameter = null);

    Task NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false);

    Task GoBackAsync();
}
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Rhizine.Services.Interfaces;

// TODO: Navigation Relay commands
public interface INavigationService
{
    event EventHandler<NavigationEventArgs> Navigated;

    bool CanGoBack { get; }

    void Initialize(Frame shellFrame);

    bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false);

    void GoBack();

    void UnsubscribeNavigation();

}
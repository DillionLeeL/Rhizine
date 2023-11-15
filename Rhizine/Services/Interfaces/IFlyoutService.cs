using Rhizine.Displays.Flyouts;

namespace Rhizine.Services
{
    public interface IFlyoutService
    {
        event Action<string> OnFlyoutOpened;

        event Action<string> OnFlyoutClosed;

        void Initialize();

        void RegisterFlyout<T>(string flyoutName) where T : FlyoutBaseViewModel, new();

        void RegisterFlyout<T>(string flyoutName, T viewModel) where T : FlyoutBaseViewModel;

        void RegisterFlyout<T>(string flyoutName, params object[] paramArray) where T : FlyoutBaseViewModel, new();

        void OpenFlyout(string flyoutName);

        void CloseFlyout(string flyoutName);
    }
}
using CommunityToolkit.Mvvm.Input;
using Rhizine.Displays.Flyouts;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using Rhizine.Views;
using System.Collections.ObjectModel;

namespace Rhizine.Services
{
    public partial class FlyoutService : IFlyoutService
    {
        private readonly ILoggingService _loggingService;
        private readonly Dictionary<string, Lazy<FlyoutBaseViewModel>> _flyouts = new();
        public ObservableCollection<FlyoutBaseViewModel> ActiveFlyouts { get; }


        public FlyoutService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            ActiveFlyouts = new ObservableCollection<FlyoutBaseViewModel>();
        }

        public event Action<string> OnFlyoutOpened;
        public event Action<string> OnFlyoutClosed;

        public void Initialize()
        {
            _loggingService.LogInformation("Registering flyouts.");
            RegisterFlyout<SettingsFlyoutViewModel>("SettingsFlyout", _loggingService);

            // Example frameflyout for either pages or URIs
            RegisterFlyout<SimpleFrameFlyoutViewModel>("WebFrameFlyout",
                                                    new Uri(@"https://www.google.com"), _loggingService);
            RegisterFlyout<SimpleFrameFlyoutViewModel>("PageFrameFlyout",
                                                    new ContentGridDetailPage(new ContentGridDetailViewModel(null)), _loggingService);
        }

        public void RegisterFlyout<T>(string flyoutName) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(() => new T());
        }
        public void RegisterFlyout<T>(string flyoutName, T viewModel) where T : FlyoutBaseViewModel
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(viewModel);
        }
        public void RegisterFlyout<T>(string flyoutName, params object[] paramArray) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(
                    () => (T)Activator.CreateInstance(typeof(T), args: paramArray));
        }
        [RelayCommand]
        public void OpenFlyout(string flyoutName)
        {
            _loggingService.LogInformation("opening" + flyoutName);
            if (_flyouts.TryGetValue(flyoutName, out var lazyFlyout))
            {
                var flyout = lazyFlyout.Value;
                if (!flyout.IsOpen)
                {
                    flyout.IsOpen = true;
                    if (!ActiveFlyouts.Contains(flyout))
                    {
                        _loggingService.LogInformation("Adding {Flyout} to active flyouts", flyout);
                        ActiveFlyouts.Add(flyout);
                    }
                }
            }
        }
        [RelayCommand]
        public void CloseFlyout(string flyoutName)
        {
            if (_flyouts.TryGetValue(flyoutName, out var lazyFlyout))
            {
                var flyout = lazyFlyout.Value;
                if (flyout.IsOpen)
                {
                    flyout.IsOpen = false;
                    ActiveFlyouts.Remove(flyout);
                }
            }
        }
    }
}

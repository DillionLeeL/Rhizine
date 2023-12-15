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

        // Triggered when a flyout is opened/closed, providing the name of the flyout
        public event Action<string> OnFlyoutOpened;

        public event Action<string> OnFlyoutClosed;

        /// <summary>
        /// Initializes the FlyoutService, registering default flyouts and performing initial setup.
        /// </summary>
        public void Initialize()
        {
            _loggingService.LogInformation("Registering flyouts.");
            _loggingService.LogInformation("Registering flyouts.");
            _loggingService.LogWarning("Registering flyouts.");
            _loggingService.LogError("Registering flyouts.");
            RegisterFlyout<SettingsFlyoutViewModel>("SettingsFlyout", _loggingService);

            // Example frameflyout for either pages or URIs
            RegisterFlyout<SimpleFrameFlyoutViewModel>("WebFrameFlyout",
                                                    new Uri(@"https://www.google.com"), _loggingService);
            RegisterFlyout<SimpleFrameFlyoutViewModel>("PageFrameFlyout",
                                                    new ContentGridDetailPage(new ContentGridDetailViewModel(null, _loggingService)), _loggingService);
        }

        /// <summary>
        /// Registers a flyout with the service using a parameterless constructor without an initial view model.
        /// </summary>
        /// <typeparam name="T">The type of the FlyoutBaseViewModel to register.</typeparam>
        /// <param name="flyoutName">The name of the flyout.</param>
        public void RegisterFlyout<T>(string flyoutName) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(() => new T());
        }

        /// <summary>
        /// Registers a flyout with the service with a specific view model.
        /// </summary>
        /// <typeparam name="T">The type of the FlyoutBaseViewModel to register.</typeparam>
        /// <param name="flyoutName">The name of the flyout.</param>
        /// <param name="viewModel">The view model to associate with the flyout.</param>
        public void RegisterFlyout<T>(string flyoutName, T viewModel) where T : FlyoutBaseViewModel
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(viewModel);
        }

        /// <summary>
        /// Registers a flyout with the service, creating the view model using specified parameters.
        /// </summary>
        /// <typeparam name="T">The type of the FlyoutBaseViewModel to register.</typeparam>
        /// <param name="flyoutName">The name of the flyout.</param>
        /// <param name="pArray">Parameters to use for creating the view model.</param>
        public void RegisterFlyout<T>(string flyoutName, params object[] pArray) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(
                    () => (T)Activator.CreateInstance(typeof(T), args: pArray));
        }

        /// <summary>
        /// Opens a flyout with the specified name.
        /// </summary>
        /// <param name="flyoutName">The name of the flyout to open.</param>
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
                        _loggingService.LogInformation($"Adding {flyout} to active flyouts");
                        ActiveFlyouts.Add(flyout);
                    }
                }
            }
        }

        /// <summary>
        /// Closes a flyout with the specified name.
        /// </summary>
        /// <param name="flyoutName">The name of the flyout to close.</param>
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
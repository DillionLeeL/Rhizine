using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Rhizine.Displays.Flyouts;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using CommunityToolkit.Mvvm.Input;
using Windows.System;
using System.Windows.Controls;
using Rhizine.Services.Interfaces;

namespace Rhizine.Services
{
    public partial class FlyoutService : IFlyoutService
    {
        private readonly ILoggingService _loggingService;

        // TODO: CHANGE TO FLYOUTCONTROL TO SEE IF THAT WORKS
        private readonly Dictionary<string, Lazy<FlyoutBaseViewModel>> _flyouts = new();
        public ObservableCollection<FlyoutBaseViewModel> ActiveFlyouts { get; }

       // private FlyoutsControl _flyoutsControl;
        //private Dictionary<string, FlyoutViewModel> flyoutMap;
        //private Dictionary<string, Action> flyoutOpenActions;

        public event Action<string> OnFlyoutOpened;
        public event Action<string> OnFlyoutClosed;

        public FlyoutService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            ActiveFlyouts = new ObservableCollection<FlyoutBaseViewModel>();
            _loggingService.LogInformation("Registering flyouts.");
            RegisterFrameFlyout3("WebFrameFlyout", new Uri(@"pack://application:,,,/Displays/Pages/WebViewPage.xaml", UriKind.RelativeOrAbsolute));
            RegisterFlyout<SettingsFlyoutViewModel>("SettingsFlyout", loggingService);
            //flyoutOpenActions = new Dictionary<string, Action>();
            //flyoutMap = new Dictionary<string, FlyoutViewModel>();
            //InitializeFlyoutActions();
        }
        
        public void Initialize() // FlyoutsControl flyoutsControl
        {
            /*
            if (_flyoutsControl == null)
            {
                _flyoutsControl = flyoutsControl;
                _flyoutsControl.ItemsSource = ActiveFlyouts;
            }
            */
        }
        //flyoutService.RegisterFlyout<SettingsFlyoutViewModel>("SettingsFlyout");
        public void RegisterFlyout<T>(string flyoutName) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(() => new T());
        }
        public void RegisterFlyout<T>(string flyoutName, T viewModel) where T : FlyoutBaseViewModel
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(viewModel);
        }
        public void RegisterFlyout<T>(string flyoutName, object param) where T : FlyoutBaseViewModel, new()
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(
                    () => (T)Activator.CreateInstance(typeof(T), new object[] { param }));
        }
        // TODO: TEST THIS 
        public void RegisterFrameFlyout2(string flyoutName, object param)
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(
                () => Activator.CreateInstance(typeof(SimpleFrameFlyoutViewModel), new object[] { param }) as FlyoutBaseViewModel);
        }
        public void RegisterFrameFlyout3(string flyoutName, object param)
        {
            _flyouts[flyoutName] = new Lazy<FlyoutBaseViewModel>(() => new SimpleFrameFlyoutViewModel(param));
        }
        [RelayCommand]
        public void OpenFlyout(string flyoutName)
        {
            _loggingService.LogInformation("opening"+flyoutName);
            if (_flyouts.TryGetValue(flyoutName, out var lazyFlyout))
            {
                var flyout = lazyFlyout.Value;
                if (!flyout.IsOpen)
                {
                    flyout.IsOpen = true;
                    if (!ActiveFlyouts.Contains(flyout))
                    {
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

        /*
        [RelayCommand]
        public void OpenFlyout(string flyoutName)
        {
            if (_flyouts.TryGetValue(flyoutName, out var lazyFlyout))
            {
                var flyout = lazyFlyout.Value;
                flyout.IsOpen = true;
                OnFlyoutOpened?.Invoke(flyout);
            }
        }
        [RelayCommand]
        public void CloseFlyout(string flyoutName)
        {
            if (_flyouts.TryGetValue(flyoutName, out var lazyFlyout))
            {
                var flyout = lazyFlyout.Value;
                flyout.IsOpen = false;
                OnFlyoutClosed?.Invoke(flyout);
            }
        }
        
        public void ShowFlyout(string flyoutName)
        {
            if (flyoutMap.TryGetValue(flyoutName, out var flyoutViewModel))
            {
                flyoutViewModel.IsOpen = true;
            }
        }
        
        public T CreateFlyout<T>(object param) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = (T)Activator.CreateInstance(typeof(T), new object[] { param });

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
        public T CreateFlyout<T>(string flyoutName, Action<T> initializeAction = null) where T : FlyoutViewModel, new()
        {
            if (flyoutMap.ContainsKey(flyoutName))
            {
                throw new InvalidOperationException($"A flyout with the name '{flyoutName}' already exists.");
            }

            var flyoutViewModel = new T();
            initializeAction?.Invoke(flyoutViewModel);

            Flyouts.Add(flyoutViewModel);
            flyoutMap[flyoutName] = flyoutViewModel;
            return flyoutViewModel;
        }


        public T CreateFlyout<T>(object param) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = (T)Activator.CreateInstance(typeof(T), new object[] { param });

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
        public T CreateFlyout<T>(Action<T> initializeAction = null) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = new T();
            initializeAction?.Invoke(flyoutViewModel);

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
        public T CreateFlyout<T>(string flyoutName, Action<T> initializeAction = null) where T : FlyoutViewModel, new()
        {
            if (flyoutMap.ContainsKey(flyoutName))
            {
                throw new InvalidOperationException($"A flyout with the name '{flyoutName}' already exists.");
            }

            var flyoutViewModel = new T();
            initializeAction?.Invoke(flyoutViewModel);

            Flyouts.Add(flyoutViewModel);
            flyoutMap[flyoutName] = flyoutViewModel;
            return flyoutViewModel;
        }
        private void InitializeFlyoutActions()
        {
            //flyoutOpenActions["SimpleFrameFlyout"] = () => OpenFrameFlyout();
        }
        
        private void OpenFrameFlyout()
        {
            var flyout = CreateFlyout<SimpleFrameFlyoutViewModel>();
            // Set up the flyout as needed
            ShowFlyout(flyout);
        }
        
        public FlyoutBaseViewModel CreateFlyout(string header)
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = new FlyoutBaseViewModel
            {
                Header = header
            };

            ActiveFlyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
            

        

        [RelayCommand]
        public void OpenFlyout(string flyoutName)
        {
            if (flyoutOpenActions.TryGetValue(flyoutName, out Action openAction))
            {
                openAction?.Invoke();
            }
        }
        [RelayCommand(CanExecute = nameof(CanFindFlyout))]
        public void ShowFlyout(FlyoutViewModel flyoutViewModel)
        {
            flyoutViewModel.IsOpen = true;
        }
        [RelayCommand(CanExecute = nameof(CanFindFlyout))]
        public void CloseFlyout(FlyoutViewModel flyoutViewModel)
        {
            flyoutViewModel.IsOpen = false;
        }
        private bool CanFindFlyout(FlyoutViewModel flyoutViewModel)
        {
            return flyoutViewModel is not null && Flyouts.Contains(flyoutViewModel);
        }
        private void ShowFlyout(string flyoutName)
        {
            if (flyoutMap.TryGetValue(flyoutName, out var flyoutViewModel))
            {
                flyoutViewModel.IsOpen = true;
            }
        }

        private void CloseFlyout(string flyoutName)
        {
            if (flyoutMap.TryGetValue(flyoutName, out var flyoutViewModel))
            {
                flyoutViewModel.IsOpen = false;
            }
        }
        */
    }
}

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

namespace Rhizine.Services
{
    public partial class FlyoutService : IFlyoutService
    {
        // TODO: CHANGE TO FLYOUTCONTROL TO SEE IF THAT WORKS
        private FlyoutsControl flyoutsControl;
        private Dictionary<string, FlyoutViewModel> flyoutMap;
        private Dictionary<string, Action> flyoutOpenActions;
        public ObservableCollection<FlyoutViewModel> Flyouts { get; }

        public FlyoutService()
        {
            Flyouts = new ObservableCollection<FlyoutViewModel>();
            flyoutOpenActions = new Dictionary<string, Action>();
            flyoutMap = new Dictionary<string, FlyoutViewModel>();
            InitializeFlyoutActions();
        }
        private void InitializeFlyoutActions()
        {
            //flyoutOpenActions["SimpleFrameFlyout"] = () => OpenFrameFlyout();
        }
        /*
        private void OpenFrameFlyout()
        {
            var flyout = CreateFlyout<SimpleFrameFlyoutViewModel>();
            // Set up the flyout as needed
            ShowFlyout(flyout);
        }
        */
        public FlyoutViewModel CreateFlyout(string header)
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = new FlyoutViewModel
            {
                Header = header
            };

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
            
        public T CreateFlyout<T>(object param) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = (T)Activator.CreateInstance(typeof(T), new object[] { param });

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
        public void CreateFlyout<T>(Action<T> initializeAction = null) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = new T();
            initializeAction?.Invoke(flyoutViewModel);

            Flyouts.Add(flyoutViewModel);
            ShowFlyout(flyoutViewModel);
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
    }
}

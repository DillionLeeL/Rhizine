using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Rhizine.Displays.Flyouts;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;

namespace Rhizine.Services
{
    public class FlyoutService : IFlyoutService
    {
        // TODO: CHANGE TO FLYOUTCONTROL TO SEE IF THAT WORKS
        [ObservableObject]
        private FlyoutsControl flyoutsControl;
        public ObservableCollection<FlyoutViewModel> Flyouts { get; }
        public FlyoutService()
        {
            Flyouts = new ObservableCollection<FlyoutViewModel>();
        }
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
        public T CreateFlyout<T>(Action<T> initializeAction = null) where T : FlyoutViewModel, new()
        {
            Console.WriteLine("Creating flyout");
            var flyoutViewModel = new T();
            initializeAction?.Invoke(flyoutViewModel);

            Flyouts.Add(flyoutViewModel);
            return flyoutViewModel;
        }
        public void ShowFlyout(FlyoutViewModel flyoutViewModel)
        {
            flyoutViewModel.IsOpen = true;
        }

        public void CloseFlyout(FlyoutViewModel flyoutViewModel)
        {
            flyoutViewModel.IsOpen = false;
        }

        // Additional methods to manage flyouts
    }
}

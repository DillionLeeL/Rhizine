using MahApps.Metro.Controls;
using Rhizine.Displays.Flyouts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rhizine.Services
{
    public interface IFlyoutService
    {
        event Action<string> OnFlyoutOpened;
        event Action<string> OnFlyoutClosed;
        void Initialize(FlyoutsControl flyoutsControl);
        void RegisterFlyout<T>(string flyoutName) where T : FlyoutBaseViewModel, new();

        void RegisterFlyout<T>(string flyoutName, T viewModel) where T : FlyoutBaseViewModel;

        //void ShowFlyout(string flyoutName);
        void OpenFlyout(string flyoutName);
        void CloseFlyout(string flyoutName);
        //T CreateFlyout<T>(object param) where T : FlyoutViewModel, new();
    }

}

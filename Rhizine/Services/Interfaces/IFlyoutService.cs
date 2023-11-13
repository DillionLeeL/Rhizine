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
        void ShowFlyout(FlyoutViewModel flyoutViewModel);

         void CloseFlyout(FlyoutViewModel flyoutViewModel);
         T CreateFlyout<T>(object param) where T : FlyoutViewModel, new();
    }

}

using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
    public partial class SimpleFrameFlyoutViewModel : FlyoutViewModel
    {
        public SimpleFrameFlyoutViewModel() { }
        public SimpleFrameFlyoutViewModel(object content)
        {
            FlyoutFrame = new Frame();
            FlyoutFrame.Navigate(content);
        }
        public SimpleFrameFlyoutViewModel(Uri source)
        {
            FlyoutFrame = new Frame();
            FlyoutFrame.Navigate(source, UriKind.RelativeOrAbsolute);
        }
        [ObservableProperty]
        private Frame _flyoutFrame;
    }
}

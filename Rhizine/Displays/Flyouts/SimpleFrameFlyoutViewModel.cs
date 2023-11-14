using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
    public partial class SimpleFrameFlyoutViewModel : FlyoutBaseViewModel
    {
        private readonly INavigationService _navigationService;
        public SimpleFrameFlyoutViewModel() { }
        public SimpleFrameFlyoutViewModel(INavigationService navigationService) 
        { 
            _navigationService = navigationService;
            _navigationService.Initialize(FlyoutFrame);
        }
        public SimpleFrameFlyoutViewModel(object content)
        {
            FlyoutFrame = new Frame();
            FlyoutFrame.Navigate(content);
        }
        public SimpleFrameFlyoutViewModel(Uri source, INavigationService navigationService)
        {
            FlyoutFrame = new Frame();
            FlyoutFrame.Navigate(source, UriKind.RelativeOrAbsolute);
        }
        [ObservableProperty]
        private Frame _flyoutFrame;
    }
}

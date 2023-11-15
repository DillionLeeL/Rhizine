using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Displays.Pages;
using Rhizine.Models;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rhizine.Displays.Flyouts
{
    public partial class SimpleFrameFlyoutViewModel : FlyoutBaseViewModel
    {
        // private readonly INavigationService _navigationService;
        private readonly ILoggingService _loggingService;
        public SimpleFrameFlyoutViewModel() { }
        public SimpleFrameFlyoutViewModel(Uri source)
        {
            CurrentUri = source;
            this.Header = "frame";
            this.Position = Position.Left;
        }
        public SimpleFrameFlyoutViewModel(Uri source, ILoggingService loggingService) 
        {
            CurrentUri = source;
            this.Header = "frame";
            this.Position = Position.Left;
            _loggingService = loggingService;
            _loggingService.LogInformation("SimpleFrameFlyoutViewModel constructor with {0}", source);
        }
        public SimpleFrameFlyoutViewModel(Page page, ILoggingService loggingService)
        {
            CurrentPage = page;
            this.Header = "frame";
            this.Position = Position.Left;
            _loggingService = loggingService;
            _loggingService.LogInformation("SimpleFrameFlyoutViewModel constructor with {0}", page);

        }
        [ObservableProperty]
        private Page _currentPage;
        [ObservableProperty]
        private Uri _currentUri;
        //[ObservableProperty]
        //private Frame _flyoutFrame;
    }
}

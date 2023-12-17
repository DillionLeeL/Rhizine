using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Rhizine.Services.Interfaces;
using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
    public partial class SimpleFrameFlyoutViewModel : FlyoutBaseViewModel
    {
        private readonly ILoggingService _loggingService;

        public SimpleFrameFlyoutViewModel()
        { }

        public SimpleFrameFlyoutViewModel(Uri source)
        {
            CurrentUri = source;
            Header = "frame";
            Position = Position.Left;
        }

        public SimpleFrameFlyoutViewModel(Uri source, ILoggingService loggingService)
        {
            CurrentUri = source;
            Header = "frame";
            Position = Position.Left;
            _loggingService = loggingService;
            _loggingService.LogDebug($"SimpleFrameFlyoutViewModel constructor with {source}");
        }

        public SimpleFrameFlyoutViewModel(Page page, ILoggingService loggingService)
        {
            CurrentPage = page;
            Header = "frame";
            Position = Position.Left;
            _loggingService = loggingService;
            _loggingService.LogInformation($"SimpleFrameFlyoutViewModel constructor with {page}");
        }

        [ObservableProperty]
        private Page _currentPage;

        [ObservableProperty]
        private Uri _currentUri;
    }
}
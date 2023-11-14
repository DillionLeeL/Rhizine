using MahApps.Metro.Controls;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Rhizine.Displays.Flyouts
{
    public partial class SettingsFlyoutViewModel : FlyoutBaseViewModel
    {
        private readonly ILoggingService _loggingService;
        public SettingsFlyoutViewModel()
        {
            this.Header = "settings";
            this.Position = Position.Left;
        }
        public SettingsFlyoutViewModel(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            this.Header = "settings";
            this.Position = Position.Left;
            _loggingService.LogInformation("SettingsFlyoutViewModel constructor");
            SettingsText = "test";
        }
        [ObservableProperty]
        private string _settingsText;
    }
}
using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Rhizine.Services.Interfaces;

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
            SettingsText = "test";
        }

        [ObservableProperty]
        private string _settingsText;
    }
}
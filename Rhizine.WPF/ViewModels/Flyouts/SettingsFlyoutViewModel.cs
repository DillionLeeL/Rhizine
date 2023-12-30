using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Rhizine.Core.Services.Interfaces;

namespace Rhizine.WPF.ViewModels.Flyouts;

public partial class SettingsFlyoutViewModel : FlyoutBaseViewModel
{
    private readonly ILoggingService _loggingService;

    public SettingsFlyoutViewModel()
    {
        Header = "settings";
        Position = Position.Left;
    }

    public SettingsFlyoutViewModel(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        Header = "settings";
        Position = Position.Left;
        SettingsText = "test";
    }

    [ObservableProperty]
    private string _settingsText;
}
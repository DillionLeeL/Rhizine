using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public partial class LandingViewModel : BaseViewModel
{
    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private IFlyoutService _flyoutService;

    public LandingViewModel(ILoggingService loggingService, IFlyoutService flyoutService)
    {
        _loggingService = loggingService;
        _flyoutService = flyoutService;

        _flyoutService.OnFlyoutOpened += FlyoutOpened;
        _flyoutService.OnFlyoutClosed += FlyoutClosed;
    }

    [RelayCommand]
    private void FlyoutOpened(string flyout)
    {
        _loggingService.LogInformation(flyout);
    }

    [RelayCommand]
    private void FlyoutClosed(string flyout)
    {
        _loggingService.LogInformation(flyout);
    }
}
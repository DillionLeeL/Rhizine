using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using System.Windows;
using System.Windows.Navigation;
using WPFBase.Displays;
using WPFBase.Displays.Popups;
using WPFBase.Services;

namespace Rhizine.Displays.Pages;

public partial class LandingViewModel : BaseViewModel
{
    private readonly ILoggingService _loggingService;
    private readonly IPageService _pageService;
    private readonly IPopupService _popupService;

    [ObservableProperty]
    private IFlyoutService _flyoutService;

    public LandingViewModel(ILoggingService loggingService, IFlyoutService flyoutService, IPageService pageService, IPopupService popupService)
    {
        _loggingService = loggingService;
        _flyoutService = flyoutService;
        _pageService = pageService;
        _popupService = popupService;

        _flyoutService.OnFlyoutOpened += FlyoutOpened;
        _flyoutService.OnFlyoutClosed += FlyoutClosed;
    }

    [RelayCommand]
    public async Task ShowPopupAsync()
    {
        try
        {
            var popupViewModel = new WaitPopupViewModel(_loggingService);
            await _popupService.ShowPopupAsync<WaitPopupViewModel, WaitPopup>(popupViewModel);
        }
        catch (Exception ex)
        {
            _loggingService.Log(ex);
        }
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
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Displays.Flyouts;
using Rhizine.Services;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public partial class LandingViewModel : BaseViewModel
{
    public LandingViewModel()
    {
        //FlyoutService.OnFlyoutOpened += FlyoutOpened;
        //FlyoutService.OnFlyoutClosed += FlyoutClosed;
    }

    

private void FlyoutOpened(string flyout)
    {
        //FlyoutService.ShowFlyout(flyout);
    }

    private void FlyoutClosed(string flyout)
    {
        //FlyoutService.CloseFlyout(flyout);
    }
}

using Microsoft.UI.Xaml;
using Rhizine.WinUI.Services.Interfaces;
using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.NavigationSource?.Content == null;
    }

    protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        await _navigationService.NavigateToAsync(typeof(MainViewModel).FullName!, args.Arguments);
    }
}
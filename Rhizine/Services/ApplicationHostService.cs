using Microsoft.Extensions.Hosting;
using Rhizine.Displays.Interfaces;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using Rhizine.Helpers;

namespace Rhizine.Services;

public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    private readonly IFlyoutService _flyoutService;
    private readonly IPersistAndRestoreService _persistAndRestoreService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private IShellWindow _shellWindow;
    private bool _isInitialized;

    public ApplicationHostService(IServiceProvider serviceProvider, IEnumerable<IActivationHandler> activationHandlers, INavigationService navigationService, IThemeSelectorService themeSelectorService,
                            IPersistAndRestoreService persistAndRestoreService, IFlyoutService flyoutService)
    {
        _serviceProvider = serviceProvider;
        _activationHandlers = activationHandlers;
        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _persistAndRestoreService = persistAndRestoreService;
        _flyoutService = flyoutService;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        if (_isInitialized) return;

        _persistAndRestoreService.RestoreData();
        _themeSelectorService.InitializeTheme();

        await HandleActivationAsync(cancellationToken);
        _isInitialized = true;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _persistAndRestoreService.PersistData();
        await Task.CompletedTask;
    }

    private async Task HandleActivationAsync(CancellationToken cancellationToken)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle());
        if (activationHandler != null)
        {
            await activationHandler.HandleAsync();
        }

        if (!System.Windows.Application.Current.Windows.OfType<IShellWindow>().Any())
        {
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _flyoutService.Initialize();
            _shellWindow.ShowWindow();
            _navigationService.NavigateTo(typeof(LandingViewModel).FullName);
        }
    }
}
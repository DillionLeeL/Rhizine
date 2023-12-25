using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhizine.Core.Helpers;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Displays.Pages;
using Rhizine.Core.Services;
using Rhizine.WPF.Services.Interfaces;
using Rhizine.WPF.Views.Interfaces;
using System.Windows.Navigation;
using Microsoft.Extensions.Options;
using Rhizine.WPF.Models;

namespace Rhizine.WPF.Services;

// TODO
public class ApplicationHostService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService<NavigationEventArgs> _navigationService;
    private readonly IFlyoutService _flyoutService;
    private readonly IPersistAndRestoreService _persistAndRestoreService;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IIdentityService _identityService;
    private readonly IToastNotificationsService _toastNotificationsService;
    private readonly AppConfig _appConfig;
    private IShellWindow _shellWindow;
    private bool _isInitialized;

    public ApplicationHostService(IServiceProvider serviceProvider, IEnumerable<IActivationHandler> activationHandlers, INavigationService<NavigationEventArgs> navigationService,
        IThemeSelectorService themeSelectorService, IPersistAndRestoreService persistAndRestoreService, IFlyoutService flyoutService,
        IToastNotificationsService toastNotificationsService, IIdentityService identityService, IOptions<AppConfig> config) // IUserDataService userDataService
    {
        _serviceProvider = serviceProvider;
        _activationHandlers = activationHandlers;
        _navigationService = navigationService;
        _themeSelectorService = themeSelectorService;
        _persistAndRestoreService = persistAndRestoreService;
        _flyoutService = flyoutService;
        _toastNotificationsService = toastNotificationsService;
        _identityService = identityService;
        _appConfig = config.Value;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await InitializeAsync();

        if (_identityService is not null)
        {
            _identityService.InitializeWithAadAndPersonalMsAccounts(_appConfig.IdentityClientId, "http://localhost");
            await _identityService.AcquireTokenSilentAsync();
        }

        await HandleActivationAsync(cancellationToken);
        await StartupAsync();
        _isInitialized = true;
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            _persistAndRestoreService.RestoreData();
            _themeSelectorService.InitializeTheme();
            await Task.CompletedTask;
        }
    }

    private async Task StartupAsync()
    {
        if (!_isInitialized)
        {
            _toastNotificationsService.ShowToastNotificationSample();
            await Task.CompletedTask;
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _persistAndRestoreService.PersistData();
        await Task.CompletedTask;
    }

    private async Task HandleActivationAsync(CancellationToken cancellationToken)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(null));
        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(null);
        }

        if (!System.Windows.Application.Current.Windows.OfType<IShellWindow>().Any())
        {
            _shellWindow = _serviceProvider.GetService(typeof(IShellWindow)) as IShellWindow;
            _navigationService.Initialize(_shellWindow.GetNavigationFrame());
            _flyoutService.Initialize();
            _shellWindow.ShowWindow();
            await _navigationService.NavigateToAsync(typeof(LandingViewModel).FullName);
        }
    }
    /// <summary>
    /// Gets the service object of the specified type.
    /// </summary>
    /// <param name="serviceType">An object that specifies the type of service object to get.</param>
    /// <returns>A service object of type <paramref name="serviceType"/>. -or- null if there is no service object of type <paramref name="serviceType"/>.</returns>
    public object GetService(Type serviceType) => _serviceProvider?.GetService(serviceType);

    /// <summary>
    /// Get service of type <typeparamref name="TService"/> from the System.IServiceProvider.
    /// </summary>
    /// <typeparam name="TService">The type of service object to get.</typeparam>
    /// <returns>A service object of type <typeparamref name="TService"/> or null if there is no such service.</returns>
    public TService? GetService<TService>() =>
        _serviceProvider is null ? default : _serviceProvider.GetService<TService>();
}
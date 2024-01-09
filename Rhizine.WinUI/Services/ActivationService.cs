using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WinUI.Activation;
using Rhizine.WinUI.Services.Interfaces;
using Rhizine.WinUI.ViewModels;
using Rhizine.WinUI.Views;

namespace Rhizine.WinUI.Services;

public class ActivationService : IActivationService
{
    private readonly ActivationHandler<LaunchActivatedEventArgs> _defaultHandler;
    private readonly IEnumerable<IActivationHandler> _activationHandlers;
    private readonly IThemeSelectorService _themeSelectorService;
    private readonly IPageService _pageService;
    private readonly ILoggingService _loggingService;
    private UIElement? _shell = null;

    public ActivationService(ActivationHandler<LaunchActivatedEventArgs> defaultHandler, IEnumerable<IActivationHandler> activationHandlers,
        IThemeSelectorService themeSelectorService, IPageService pageService, ILoggingService loggingService)
    {
        _defaultHandler = defaultHandler;
        _activationHandlers = activationHandlers;
        _themeSelectorService = themeSelectorService;
        _pageService = pageService;
        _loggingService = loggingService;
    }

    public async Task ActivateAsync(object activationArgs)
    {
        try
        {
            // Execute tasks before activation.
            await InitializeAsync();

            // Set the MainWindow Content.
            if (App.MainWindow.Content == null)
            {
                _shell = App.GetService<ShellPage>();
                App.MainWindow.Content = _shell ?? new Frame();
            }

            // Handle activation via ActivationHandlers.
            await HandleActivationAsync(activationArgs);

            // Activate the MainWindow.
            App.MainWindow.Activate();

            // Execute tasks after activation.
            await StartupAsync();
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex);
            throw;
        }
    }

    private async Task HandleActivationAsync(object activationArgs)
    {
        var activationHandler = _activationHandlers.FirstOrDefault(h => h.CanHandle(activationArgs));

        if (activationHandler != null)
        {
            await activationHandler.HandleAsync(activationArgs);
        }

        if (_defaultHandler.CanHandle(activationArgs))
        {
            await _defaultHandler.HandleAsync(activationArgs);
        }
    }

    private async Task InitializeAsync()
    {
        _pageService.Register<MainViewModel, MainPage>();
        _pageService.Register<WebViewViewModel, WebViewPage>();
        _pageService.Register<DataGridViewModel, DataGridPage>();
        _pageService.Register<ContentGridViewModel, ContentGridPage>();
        _pageService.Register<ContentGridDetailViewModel, ContentGridDetailPage>();
        _pageService.Register<ListDetailsViewModel, ListDetailsPage>();
        _pageService.Register<SettingsViewModel, SettingsPage>();
        
        await _themeSelectorService.InitializeAsync().ConfigureAwait(false);
    }

    // Empty for now
    private async Task StartupAsync()
    {
        await Task.CompletedTask;
    }
}
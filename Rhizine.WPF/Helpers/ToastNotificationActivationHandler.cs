using System.Windows;
using System.Windows.Navigation;
using Microsoft.Extensions.Configuration;
using Rhizine.Core.Helpers;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WPF.Services;
using Rhizine.WPF.Services.Interfaces;
using Rhizine.WPF.Views.Interfaces;

// TODO
// https://docs.microsoft.com/windows/apps/design/shell/tiles-and-notifications/send-local-toast?tabs=desktop
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WPF/features/toast-notifications.md
namespace Rhizine.WPF.Helpers;

public class ToastNotificationActivationHandler : IActivationHandler
{
    public const string ActivationArguments = "ToastNotificationActivationArguments";

    private readonly IConfiguration _config;
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService<NavigationEventArgs> _navigationService;

    public ToastNotificationActivationHandler(IConfiguration config, IServiceProvider serviceProvider, INavigationService<NavigationEventArgs> navigationService)
    {
        _config = config;
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;
    }

    public bool CanHandle(object args = null)
    {
        return !string.IsNullOrEmpty(_config[ActivationArguments]);
    }

    public async Task HandleAsync(object args = null)
    {
        if (Application.Current.Windows.OfType<IShellWindow>().Count() == 0)
        {
            // Here you can get an instance of the ShellWindow and choose navigate
            // to a specific page depending on the toast notification arguments
        }
        else
        {
            Application.Current.MainWindow.Activate();
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
        }

        await Task.CompletedTask;
    }
}

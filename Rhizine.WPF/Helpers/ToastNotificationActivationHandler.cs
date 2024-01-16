using Microsoft.Extensions.Configuration;
using Rhizine.Core.Helpers;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WPF.Views.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

// TODO
// https://docs.microsoft.com/windows/apps/design/shell/tiles-and-notifications/send-local-toast?tabs=desktop
// https://github.com/microsoft/TemplateStudio/blob/main/docs/WPF/features/toast-notifications.md
namespace Rhizine.WPF.Helpers;

public class ToastNotificationActivationHandler(IConfiguration config, IServiceProvider serviceProvider, INavigationService<Frame, NavigationEventArgs> navigationService) : IActivationHandler
{
    public const string ActivationArguments = "ToastNotificationActivationArguments";

    private readonly IConfiguration _config = config;
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly INavigationService<Frame, NavigationEventArgs> _navigationService = navigationService;

    public bool CanHandle(object args = null)
    {
        return !string.IsNullOrEmpty(_config[ActivationArguments]);
    }

    public async Task HandleAsync(object args = null)
    {
        if (!Application.Current.Windows.OfType<IShellWindow>().Any())
        {
            // Here you can get an instance of the ShellWindow and choose navigate
            // to a specific page depending on the toast notification arguments
        }
        else
        {
            Application.Current.MainWindow.Activate();
            if (Application.Current.MainWindow.WindowState == WindowState.Minimized)
            {
                Application.Current.MainWindow.SetCurrentValue(Window.WindowStateProperty, WindowState.Normal);
            }
        }

        await Task.CompletedTask;
    }
}
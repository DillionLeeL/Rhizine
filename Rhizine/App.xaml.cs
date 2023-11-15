using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rhizine.Displays.Interfaces;
using Rhizine.Displays.Pages;
using Rhizine.Displays.Windows;
using Rhizine.Models;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using Rhizine.Views;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Rhizine;

public partial class App : Application
{
    private IHost _host;
    public App() { }

    public T GetService<T>() where T : class => _host.Services.GetService(typeof(T)) as T;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        // NOTE: Entry Assembly logic removed from app configure when building as single exe
        // TODO: make this optional
        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

        _host = Host.CreateDefaultBuilder(e.Args)
                /* Looks for the .dll which will fail when embedded in .exe
                    .ConfigureAppConfiguration(c =>
                    {
                        c.SetBasePath(appLocation);
                    })
                */
                .ConfigureServices(ConfigureServices)
                .Build();

        await _host.StartAsync();
    }

    // When adding a service, add it below and in Services\ApplicationHostService.cs
    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // App Host
        services.AddHostedService<ApplicationHostService>();
        //services.AddLogging(configure => configure.AddConsole()); // Add other logging providers as needed

        // Services
        services.AddSingleton<IFileService, FileService>();
        services.AddSingleton<ILoggingService, LoggingService>();
        services.AddSingleton<IApplicationInfoService, ApplicationInfoService>();
        services.AddSingleton<IPersistAndRestoreService, PersistAndRestoreService>();
        services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
        services.AddSingleton<ISampleDataService, SampleDataService>();
        services.AddSingleton<ISystemService, SystemService>();
        services.AddSingleton<IPageService, PageService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IFlyoutService, FlyoutService>();

        // Views and ViewModels
        services.AddTransient<IShellWindow, ShellWindow>();
        services.AddTransient<ShellViewModel>();

        services.AddTransient<LandingViewModel>();
        services.AddTransient<LandingPage>();

        services.AddTransient<WebViewViewModel>();
        services.AddTransient<WebViewPage>();

        services.AddTransient<DataGridViewModel>();
        services.AddTransient<DataGridPage>();

        services.AddTransient<ContentGridViewModel>();
        services.AddTransient<ContentGridPage>();

        services.AddTransient<ContentGridDetailViewModel>();
        services.AddTransient<ContentGridDetailPage>();

        services.AddTransient<ListDetailsViewModel>();
        services.AddTransient<ListDetailsPage>();

        services.AddTransient<SettingsViewModel>();
        services.AddTransient<SettingsPage>();

        // Configuration
        services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
    }

    private async void OnExit(object sender, ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        _host = null;
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        // TODO: Please log and handle the exception as appropriate to your scenario
        // For more info see https://docs.microsoft.com/dotnet/api/system.windows.application.dispatcherunhandledexception?view=netcore-3.0
    }
}

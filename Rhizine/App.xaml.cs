using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rhizine.Displays.Interfaces;
using Rhizine.Displays.Pages;
using Rhizine.Displays.Windows;
using Rhizine.Models;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using Rhizine.Views;
using Serilog;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;

namespace Rhizine;

public partial class App : Application
{
    private IHost _host;

    public T GetService<T>() where T : class => _host.Services.GetService(typeof(T)) as T;

    private async void OnStartup(object sender, StartupEventArgs e)
    {
        // NOTE: Entry Assembly logic removed from app configure when building as single exe
        // TODO: make single exe .ConfigureAppConfiguration optional
        /* Looks for the .dll which will fail when embedded in .exe
            .ConfigureAppConfiguration(c =>
            {
                c.SetBasePath(appLocation);
            })
        */
        // Configure Serilog
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build())
            .CreateLogger();

        var appLocation = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);

        _host = Host.CreateDefaultBuilder()
            .UseSerilog() // Use Serilog for logging
            .ConfigureServices(ConfigureServices)
            .Build();

        await _host.StartAsync();
    }

    // When adding a service, add it below and in Services\ApplicationHostService.cs
    private void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        // App Host
        services.AddHostedService<ApplicationHostService>();
        //services.AddLogging(configure => configure.AddConsole()); // default console
        //services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        // Add configuration to DI
        var configuration = context.Configuration;
        services.AddSingleton(configuration);
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
        services.AddSingleton<IPopupService, PopupService>();

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
        services.Configure<AppConfig>(configuration.GetSection(nameof(AppConfig)));
    }

    protected async void OnExit(object sender, ExitEventArgs e)
    {
        await Log.CloseAndFlushAsync();

        await _host.StopAsync();
        _host.Dispose();
    }

    private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        GetService<ILoggingService>().HandleGlobalException(e.Exception);
        e.Handled = true;
    }
}
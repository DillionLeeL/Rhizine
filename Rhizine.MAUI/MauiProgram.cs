using CommunityToolkit.Maui.Maps;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Foldable;
using Rhizine.Core.Services;
using Rhizine.Core.Services.Interfaces;
using Rhizine.MAUI.Services;
using CommunityToolkit.Maui.Core;
using Serilog;

namespace Rhizine.MAUI
{
    public static partial class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            {
                Console.WriteLine(e);
            };

            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateLogger();


            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>()
                   // .UseFoldable()
                   // .UseMauiMaps()
                   // .UseMauiCommunityToolkitMaps("<BING_MAPS_API_KEY_HERE>") // To generate a Bing Maps API Key, visit https://www.bingmapsportal.com/
                   .UseMauiCommunityToolkit()
                   .UseMauiCommunityToolkitMarkup()
                   .UseMauiCommunityToolkitMediaElement()
                   .ConfigureFonts(fonts =>
                   {
                       fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                       fonts.AddFont("OpenSans-SemiBold.ttf", "OpenSansSemiBold");
                   });

            builder.Services.AddSingleton<IDialogService, DialogService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<ILoggingService, LoggingService>();
            builder.Services.AddSingleton<IThemeSelectorService, ThemeSelectorService>();
            builder.Services.AddSingleton<ISampleDataService, SampleDataService>();
            // CommunityToolkit.Maui PopupService
            builder.Services.AddSingleton<IPopupService, PopupService>();

            builder.Services.AddSingleton<EventsViewModel>();
            builder.Services.AddSingleton<EventsPage>();
            builder.Services.AddSingleton<WebViewViewModel>();
            builder.Services.AddSingleton<WebViewPage>();
            builder.Services.AddSingleton<ListDetailsViewModel>();
            builder.Services.AddSingleton<ListDetailsPage>();
            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<SearchPage>();
            builder.Services.AddSingleton<SettingsViewModel>();
            builder.Services.AddSingleton<SettingsPage>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<NewEventViewModel>();
            builder.Services.AddTransient<NewEventPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Logging.AddSerilog();
            return builder.Build();
        }
    }
}

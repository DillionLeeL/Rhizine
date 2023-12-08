using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using Rhizine.Views;
using System.Collections.Concurrent;
using System.Windows.Controls;

namespace Rhizine.Services;

public class PageService : IPageService
{
    private readonly ConcurrentDictionary<string, Lazy<Page>> _pagesCache = new ConcurrentDictionary<string, Lazy<Page>>();
    private readonly ConcurrentDictionary<string, Type> _pageTypes = new ConcurrentDictionary<string, Type>();
    private readonly IServiceProvider _serviceProvider;

    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Initialize();
    }
    private void Initialize()
    {
        Register<LandingViewModel, LandingPage>();
        Register<WebViewViewModel, WebViewPage>();
        Register<DataGridViewModel, DataGridPage>();
        Register<ContentGridViewModel, ContentGridPage>();
        Register<ContentGridDetailViewModel, ContentGridDetailPage>();
        Register<ListDetailsViewModel, ListDetailsPage>();
        Register<SettingsViewModel, SettingsPage>();
    }
    public Page GetPage(string key)
    {
        if (!_pagesCache.TryGetValue(key, out var lazyPage))
        {
            if (!_pageTypes.TryGetValue(key, out var pageType))
            {
                throw new ArgumentException($"Page not found for key {key}. Ensure it is configured properly in PageService.");
            }

            lazyPage = new Lazy<Page>(() =>
            {
                if (_serviceProvider.GetService(pageType) is Page page)
                {
                    return page;
                }

                throw new InvalidOperationException($"The requested service of type {pageType.FullName} could not be resolved as a Page.");
            });

            _pagesCache[key] = lazyPage;
        }

        return lazyPage.Value;
    }
    private void Register<VM, V>()
            where VM : ObservableObject
            where V : Page
    {
        var key = typeof(VM).FullName;
        var type = typeof(V);

        if (!_pageTypes.TryAdd(key, type))
        {
            throw new ArgumentException($"The key {key} is already configured in PageService.");
        }
    }
}

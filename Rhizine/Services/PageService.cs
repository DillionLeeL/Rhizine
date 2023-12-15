using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Displays.Pages;
using Rhizine.Services.Interfaces;
using Rhizine.Views;
using System.Collections.Concurrent;
using System.Windows.Controls;

namespace Rhizine.Services;

public class PageService : IPageService
{
    private readonly ConcurrentDictionary<string, Lazy<Page>> _pagesCache = new();
    private readonly ConcurrentDictionary<string, Type> _pageTypes = new();
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// Initializes a new instance of the PageService with a given service provider.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve page instances.</param>
    /// <exception cref="ArgumentNullException">Thrown if serviceProvider is null.</exception>
    public PageService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        Initialize();
    }

    /// <summary>
    /// Initializes the page service by registering known ViewModel-Page mappings.
    /// </summary>
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

    /// <summary>
    /// Retrieves a Page instance associated with a given ViewModel key.
    /// </summary>
    /// <param name="key">The key (usually ViewModel type name) to look up the page instance.</param>
    /// <returns>A Page instance associated with the given key.</returns>
    /// <exception cref="ArgumentException">Thrown if the key is not found in the page mappings.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the resolved service is not a Page.</exception>
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

    /// <summary>
    /// Registers a ViewModel-Page mapping in the service.
    /// </summary>
    /// <typeparam name="VM">The type of the ViewModel.</typeparam>
    /// <typeparam name="V">The type of the Page.</typeparam>
    /// <exception cref="ArgumentException">Thrown if the ViewModel type is already registered.</exception>
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
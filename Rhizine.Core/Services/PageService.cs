using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Services.Interfaces;
using System.Collections.Concurrent;

namespace Rhizine.Core.Services;

public class PageService<TPage>(IServiceProvider serviceProvider) : IPageService<TPage>
{
    private readonly ConcurrentDictionary<string, Lazy<TPage>> _pagesCache = new();
    private readonly ConcurrentDictionary<string, Type> _pageTypes = new();
    private readonly IServiceProvider _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    /// <summary>
    /// Retrieves a Page instance associated with a given ViewModel key.
    /// </summary>
    /// <param name="key">The key (usually ViewModel type name) to look up the page instance.</param>
    /// <returns>A Page instance associated with the given key.</returns>
    /// <exception cref="ArgumentException">Thrown if the key is not found in the page mappings.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the resolved service is not a Page.</exception>
    public TPage GetPage(string key)
    {
        if (!_pagesCache.TryGetValue(key, out var lazyPage))
        {
            if (!_pageTypes.TryGetValue(key, out var pageType))
            {
                throw new ArgumentException($"Page not found for key {key}. Ensure it is configured properly in PageService.");
            }

            lazyPage = new Lazy<TPage>(() =>
            {
                if (_serviceProvider.GetService(pageType) is TPage page)
                {
                    return page;
                }

                throw new InvalidOperationException($"The requested service of type {pageType.FullName} could not be resolved as a Page.");
            });

            _pagesCache[key] = lazyPage;
        }

        return lazyPage.Value;
    }
    public TPage GetPage<TViewModel>()
    {
        if (typeof(TViewModel).FullName is string key)
        {
            return GetPage(key);
        }
        throw new ArgumentNullException($"Page not found for ViewModel {typeof(TViewModel)}. Ensure it is configured properly in PageService.");
    }
    /// <summary>
    /// Registers a ViewModel-Page mapping in the service.
    /// </summary>
    /// <typeparam name="VM">The type of the ViewModel.</typeparam>
    /// <typeparam name="V">The type of the Page.</typeparam>
    /// <exception cref="ArgumentException">Thrown if the ViewModel type is already registered.</exception>
    public void Register<VM, V>()
            where VM : ObservableObject
            where V : TPage
    {
        var key = typeof(VM).FullName;
        var type = typeof(V);

        if (!_pageTypes.TryAdd(key, type))
        {
            throw new ArgumentException($"The key {key} is already configured in PageService.");
        }
    }
}
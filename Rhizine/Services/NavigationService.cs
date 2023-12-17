using Rhizine.Displays.Interfaces;
using Rhizine.Services.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Rhizine.Services;

public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private readonly ILoggingService _loggingService;
    private Frame _frame;
    private object _lastParameterUsed;

    public event EventHandler<NavigationEventArgs> Navigated;

    public bool CanGoBack => _frame?.CanGoBack ?? false;

    public NavigationService(IPageService pageService, ILoggingService loggingService)
    {
        _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
    }

    /// <summary>
    /// Initializes the navigation service with the specified frame.
    /// </summary>
    /// <param name="shellFrame">The frame to be used for navigation.</param>
    public void Initialize(Frame shellFrame)
    {
        _frame = shellFrame ?? throw new ArgumentNullException(nameof(shellFrame));
        _frame.Navigated += OnNavigated;
    }

    /// <summary>
    /// Unsubscribes from the Navigated event and clears the navigation history.
    /// </summary>
    public void UnsubscribeNavigation()
    {
        _frame.Navigated -= OnNavigated;
        _frame?.ClearNavigation();
        _frame = null;
    }

    /// <summary>
    /// Navigates to the previous page in the navigation history, if available.
    /// </summary>
    public void GoBack()
    {
        _loggingService.LogDebug("Navigating back");
        if (!CanGoBack) return;

        var vmBeforeNavigation = _frame.GetDataContext();
        _frame.GoBack();

        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            _ = navigationAware.OnNavigatedFrom();
        }
    }

    /// <summary>
    /// Navigates to a specified page.
    /// </summary>
    /// <param name="pageKey">The key of the page to navigate to.</param>
    /// <param name="parameter">The parameter to pass to the new page.</param>
    /// <param name="clearNavigation">Indicates whether to clear the navigation history.</param>
    /// <returns>True if navigation is successful, false otherwise.</returns>
    public bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));

        var page = _pageService.GetPage(pageKey);
        if (page == null) return false;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return false;

        _frame.SetCurrentValue(FrameworkElement.TagProperty, clearNavigation);

        try
        {
            if (!_frame.Navigate(page, parameter)) return false;

            HandleNavigationAware(page, parameter);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Asynchronously navigates to a specified page.
    /// </summary>
    /// <param name="pageKey">The key of the page to navigate to.</param>
    /// <param name="parameter">The parameter to pass to the new page.</param>
    /// <param name="clearNavigation">Indicates whether to clear the navigation history.</param>
    /// <returns>True if navigation is successful, false otherwise.</returns>
    public async Task<bool> NavigateToAsync(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));

        var page = _pageService.GetPage(pageKey);
        if (page == null) return false;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return false;

        _frame.SetCurrentValue(FrameworkElement.TagProperty, clearNavigation);

        try
        {
            var navigateResult = await Application.Current.Dispatcher.InvokeAsync(() => _frame.Navigate(page, parameter));

            if (!navigateResult) return false;

            HandleNavigationAware(page, parameter);
            return true;
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex);
            return false;
        }
    }

    private void HandleNavigationAware(object page, object parameter)
    {
        if (page is INavigationAware navigationAware)
        {
            _lastParameterUsed = parameter;
            _ = navigationAware.OnNavigatedTo(parameter);
        }
    }

    private bool ShouldNavigateTo(Type pageType, object parameter) =>
        _frame.Content?.GetType() != pageType || !Equals(parameter, _lastParameterUsed);

    /// <summary>
    /// Event handler for the Navigated event. It clears the navigation history if required and
    /// notifies the navigated-to page about the navigation.
    /// </summary>
    /// <param name="sender">The frame that has navigated.</param>
    /// <param name="e">Event arguments containing information about the navigation.</param>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame { Tag: bool clearNavigation } frame)
        {
            if (clearNavigation)
            {
                frame.ClearNavigation();
            }

            Navigated?.Invoke(sender, e);

            if (frame.GetDataContext() is INavigationAware navigationAware)
            {
                _ = navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }
}

/* TODO
public bool NavigateTo<T>(object parameter = null, bool clearNavigation = false) where T : Page
{
    var pageKey = typeof(T).Name;
    if (_frame.Content?.GetType() != typeof(T) || (parameter != null && !parameter.Equals(_lastParameterUsed)))
    {
        _frame.SetCurrentValue(System.Windows.FrameworkElement.TagProperty, clearNavigation);
        var page = _pageService.GetPage<T>();
        var navigated = _frame.Navigate(page, parameter);
        if (navigated)
        {
            _lastParameterUsed = parameter;
            ((INavigationAware)_frame.Content)?.OnLeavingFromPage(_lastParameterUsed?.GetType());
        }
        return navigated;
    }
    return false;
}
*/
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Rhizine.WPF.Services;

/// <summary>
/// Initializes a new instance of the NavigationService class.
/// </summary>
/// <param name="pageService">The service for managing pages.</param>
/// <param name="loggingService">The service for logging errors and information.</param>
public class NavigationService(IPageService pageService, ILoggingService loggingService) : INavigationService
{
    private readonly IPageService _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
    private readonly ILoggingService _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));

    public event EventHandler<NavigationEventArgs> Navigated;
    private Frame _frame;
    public Frame NavigationSource => _frame;
    private object _lastParameterUsed;

    [MemberNotNullWhen(true, nameof(Frame), nameof(_frame))]
    public bool CanGoBack => _frame?.CanGoBack ?? false;

    /// <summary>
    /// Initializes the navigation service with the specified frame.
    /// </summary>
    /// <param name="source">The frame to be used for navigation.</param>
    public void Initialize(Frame source)
    {
        _frame = source;
        _frame.Navigated += OnNavigated;
    }

    /// <summary>
    /// Unsubscribes from the Navigated event and clears the navigation history.
    /// </summary>
    public void UnsubscribeNavigation()
    {
        if (_frame == null) return;

        _frame.Navigated -= OnNavigated;
        NavigationSource.ClearNavigation();
        _frame = null;
    }

    /// <summary>
    /// Navigates to the previous page in the navigation history, if available, and notifies involved view models about the navigation change.
    /// </summary>
    /// <returns><c>true</c> if navigation was successful; otherwise, <c>false</c>.</returns>
    public bool GoBack()
    {
        if (!CanGoBack) return false;

        var vmBeforeNavigation = NavigationSource.GetDataContext();
        NavigationSource.GoBack();
        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        return true;
    }

    /// <summary>
    /// Navigates to the previous page in the navigation history, if available, and notifies involved view models about the navigation change.
    /// </summary>
    public async Task GoBackAsync()
    {
        await _loggingService.LogDebugAsync("Navigating back");
        if (!CanGoBack) return;

        var vmBeforeNavigation = NavigationSource.GetDataContext();
        NavigationSource.GoBack();

        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
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

        _loggingService.LogDebug($"Navigating to {pageKey}");

        var page = _pageService.GetPage(pageKey);
        if (page == null) return false;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return false;

        NavigationSource.SetCurrentValue(FrameworkElement.TagProperty, clearNavigation);

        try
        {
            if (!NavigationSource.Navigate(page, parameter)) return false;

            HandleNavigationAware(page, parameter);
            return true;
        }
        catch (Exception ex)
        {
            _loggingService.Log(ex, $"Failed to navigate to {pageKey}");
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
    public async Task NavigateToAsync(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));

        await _loggingService.LogDebugAsync($"Navigating to {pageKey}");

        var page = _pageService.GetPage(pageKey);
        if (page == null) return;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return;

        NavigationSource.SetCurrentValue(FrameworkElement.TagProperty, clearNavigation);

        try
        {
            var navigateResult = await Application.Current.Dispatcher.InvokeAsync(() => NavigationSource.Navigate(page, parameter));

            if (!navigateResult) return;

            HandleNavigationAware(page, parameter);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, $"Failed to navigate to {pageKey}");
        }
    }

    private void HandleNavigationAware(object page, object parameter)
    {
        if (page is INavigationAware navigationAware)
        {
            _lastParameterUsed = parameter;
            navigationAware.OnNavigatedTo(parameter);
        }
    }

    private bool ShouldNavigateTo(Type pageType, object parameter) =>
        _frame.Content?.GetType() != pageType || (parameter?.Equals(_lastParameterUsed) == false);

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
                navigationAware.OnNavigatedTo(e.ExtraData);
            }
        }
    }
}
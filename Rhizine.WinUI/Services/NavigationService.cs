using CommunityToolkit.WinUI.UI.Animations;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services.Interfaces;
using Rhizine.WinUI.Helpers;
using Rhizine.WinUI.Services.Interfaces;
using System;
using System.Diagnostics.CodeAnalysis;
using IPageService = Rhizine.Core.Services.Interfaces.IPageService<Microsoft.UI.Xaml.Controls.Page>;

// INavigationService alias is defined in Rhizine.WinUI/Usings.cs
// Very similar to Rhizine.WPF/Services/NavigationService.cs
namespace Rhizine.WinUI.Services;


/// <summary>
/// Provides navigation services for the application, managing navigation between pages and handling navigation-related events.
/// </summary>
public class NavigationService : INavigationService
{
    private readonly IPageService _pageService;
    private readonly ILoggingService _loggingService;
    private object? _lastParameterUsed;
    private Frame? _frame;
    //public Frame NavigationSource => _frame;

    /// <summary>
    /// Initializes a new instance of the <see cref="NavigationService"/> class.
    /// </summary>
    /// <param name="pageService">Service responsible for retrieving page instances.</param>
    /// <param name="loggingService">Service used for logging.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="pageService"/> or <paramref name="loggingService"/> is null.</exception>
    public NavigationService(IPageService pageService, ILoggingService loggingService)
    {
        _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
    }

    /// <summary>
    /// Occurs when a navigation has been completed.
    /// </summary>
    public event EventHandler<NavigationEventArgs> Navigated;
    /// <summary>
    /// Gets or sets the Frame used for navigation. Ensures navigation events are registered and unregistered appropriately.
    /// </summary>
    public Frame? NavigationSource
    {
        get
        {
            if (_frame == null)
            {
                _frame = App.MainWindow.Content as Frame;
                RegisterFrameEvents();
            }

            return _frame;
        }
        set
        {
            if (_frame != value)
            {
                UnregisterFrameEvents();
                _frame = value;
                RegisterFrameEvents();
            }
        }
    }

    /// <summary>
    /// Gets a value indicating whether the navigation service can navigate back.
    /// </summary>
    [MemberNotNullWhen(true, nameof(NavigationSource), nameof(_frame))]
    public bool CanGoBack => NavigationSource?.CanGoBack == true;

    /// <summary>
    /// Initializes the navigation service with the specified frame.
    /// </summary>
    /// <param name="source">The frame to be used for navigation.</param>
    public void Initialize(Frame source)
    {
        NavigationSource = source;
    }

    private void RegisterFrameEvents()
    {
        if (_frame != null)
        {
            _frame.Navigated += OnNavigated;
        }
    }

    private void UnregisterFrameEvents()
    {
        if (_frame == null) return;

        _frame.Navigated -= OnNavigated;
    }

    /// <summary>
    /// Navigates back if possible, and notifies involved view models about the navigation change.
    /// </summary>
    /// <returns><c>true</c> if navigation was successful; otherwise, <c>false</c>.</returns>
    public bool GoBack()
    {
        if (!CanGoBack) return false;

        var vmBeforeNavigation = _frame.GetPageViewModel();
        _frame.GoBack();
        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }

        return true;
    }

    /// <summary>
    /// Navigates to the specified page and passes a parameter if provided.
    /// </summary>
    /// <param name="pageKey">The key identifying the page to navigate to.</param>
    /// <param name="parameter">The parameter to pass to the target page.</param>
    /// <param name="clearNavigation">Indicates whether to clear the navigation history.</param>
    /// <returns><c>true</c> if navigation was successful; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="pageKey"/> is null or whitespace.</exception>
    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));
        _loggingService.Log($"Navigating to {pageKey}");

        var page = _pageService.GetPage(pageKey);

        if (page == null)
        {
            _loggingService.Log($"Navigation to {pageKey} failed");
            return false;
        }

        var pageType = page.GetType();
        if (!ShouldNavigateTo(pageType, parameter!))
        {
            _loggingService.Log($"Navigation to {page.Name} cancelled");
            return false;
        }

        _frame!.Tag = clearNavigation;
        var vmBeforeNavigation = _frame.GetPageViewModel();
        // TODO: failing on navigation to grid details
        var navigated = _frame.Navigate(pageType, parameter);
        if (navigated)
        {
            _lastParameterUsed = parameter;
            if (vmBeforeNavigation is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedFrom();
            }
            
        }
        else
        {
            _loggingService.Log($"Navigation to {page.Name} failed");
        }

        return navigated;
    }
    private void HandleNavigationAware(object page, object parameter)
    {
        if (page is INavigationAware navigationAware)
        {
            _lastParameterUsed = parameter;
            navigationAware.OnNavigatedTo(parameter);
        }
    }
    /// <summary>
    /// Determines whether navigation to a specified page should occur based on the current page and the parameters.
    /// </summary>
    /// <param name="pageType">Type of the page to navigate to.</param>
    /// <param name="parameter">Parameter passed for the navigation.</param>
    /// <returns><c>true</c> if navigation should occur; otherwise, <c>false</c>.</returns>
    private bool ShouldNavigateTo(Type pageType, object parameter) =>
        _frame?.Content?.GetType() != pageType || (parameter?.Equals(_lastParameterUsed) == false);

    /// <summary>
    /// Handles the Navigated event of the frame. Invokes the Navigated event and notifies the view model of the navigation.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">Event data that provides information about the navigation.</param>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame)
        {
            var clearNavigation = (bool)frame.Tag;
            if (clearNavigation)
            {
                frame.BackStack.Clear();
            }

            if (frame.GetPageViewModel() is INavigationAware navigationAware)
            {
                navigationAware.OnNavigatedTo(e.Parameter);
            }

            Navigated?.Invoke(sender, e);
        }
    }
    /// <summary>
    /// Not Implemented.
    /// </summary>
    public Task NavigateToAsync(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        throw new NotImplementedException();
    }
    /// <summary>
    /// Not Implemented.
    /// </summary>
    public Task GoBackAsync()
    {
        throw new NotImplementedException();
    }
    /*
/// <summary>
/// Sets the list data item for the next connected animation in the Frame.
/// </summary>
/// <param name="item">The data item to use for the next connected animation.</param>
public void SetListDataItemForNextConnectedAnimation(object item)
{
   Frame.SetListDataItemForNextConnectedAnimation(item); }
*/
}
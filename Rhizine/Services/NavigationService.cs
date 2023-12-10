using CommunityToolkit.Mvvm.Messaging;
using Microsoft.VisualStudio.Shell;
using Rhizine.Displays.Interfaces;
using Rhizine.Messages;
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

    public void Initialize(Frame shellFrame)
    {
        _frame = shellFrame ?? throw new ArgumentNullException(nameof(shellFrame));
        _frame.Navigated += OnNavigated;
    }

    public void UnsubscribeNavigation()
    {
        _frame.Navigated -= OnNavigated;
        _frame?.ClearNavigation();
        _frame = null;
    }

    public void GoBack()
    {
        if (!CanGoBack) return;

        var vmBeforeNavigation = _frame.GetDataContext();
        _frame.GoBack();

        if (vmBeforeNavigation is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }
    }

    public bool NavigateTo(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));

        var page = _pageService.GetPage(pageKey);
        if (page == null) return false;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return false;

        _frame.SetCurrentValue(System.Windows.FrameworkElement.TagProperty, clearNavigation);

        try
        {
            if (!_frame.Navigate(page, parameter)) return false;

            HandleNavigationAware(page, parameter);
            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<bool> NavigateToAsync(string pageKey, object parameter = null, bool clearNavigation = false)
    {
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace.", nameof(pageKey));

        var page = _pageService.GetPage(pageKey);
        if (page == null) return false;

        if (!ShouldNavigateTo(page.GetType(), parameter)) return false;

        _frame.SetCurrentValue(System.Windows.FrameworkElement.TagProperty, clearNavigation);

        try
        {
            var navigateResult = await Application.Current.Dispatcher.InvokeAsync(() => _frame.Navigate(page, parameter));
          
            if (!navigateResult) return false;

            HandleNavigationAware(page, parameter);
            return true;
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex);
            return false;
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
        _frame.Content?.GetType() != pageType || !object.Equals(parameter, _lastParameterUsed);

    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame && frame.Tag is bool clearNavigation)
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
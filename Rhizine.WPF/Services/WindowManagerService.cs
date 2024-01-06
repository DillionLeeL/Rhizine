using MahApps.Metro.Controls;
using Rhizine.Core.Models.Interfaces;
using Rhizine.WPF.Services.Interfaces;
using Rhizine.WPF.Views.Interfaces;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Rhizine.WPF.Services;

/// <summary>
/// A service class responsible for managing windows in a WPF application.
/// This service handles the creation and management of new windows and dialog windows,
/// as well as navigation between different pages within these windows.
/// </summary>
public class WindowManagerService : IWindowManagerService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPageService _pageService;

    public Window MainWindow => Application.Current.MainWindow;

    /// <summary>
    /// Initializes a new instance of the WindowManagerService class.
    /// </summary>
    /// <param name="serviceProvider">The service provider for dependency injection.</param>
    /// <param name="pageService">The service for managing pages.</param>
    public WindowManagerService(IServiceProvider serviceProvider, IPageService pageService)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _pageService = pageService ?? throw new ArgumentNullException(nameof(pageService));
    }

    /// <summary>
    /// Opens a specified page in a new window or activates it if it's already open.
    /// </summary>
    /// <param name="pageKey">The key identifying the page to open.</param>
    /// <param name="parameter">Optional parameter to pass to the page.</param>
    public void OpenInNewWindow(string pageKey, object parameter = null)
    {
        ValidateKey(pageKey);

        var window = GetWindow(pageKey);
        if (window != null)
        {
            window.Activate();
            return;
        }

        window = CreateNewWindow();
        var frame = InitializeFrame(window);

        NavigateToPage(frame, pageKey, parameter, window);
    }

    /// <summary>
    /// Opens a specified page in a modal dialog window.
    /// </summary>
    /// <param name="pageKey">The key identifying the page to open.</param>
    /// <param name="parameter">Optional parameter to pass to the page.</param>
    /// <returns>A nullable boolean indicating how the dialog was closed.</returns>
    public bool? OpenInDialog(string pageKey, object parameter = null)
    {
        var shellWindow = _serviceProvider.GetService(typeof(IShellDialogWindow)) as Window ?? throw new InvalidOperationException("Shell dialog window not found.");
        var frame = ((IShellDialogWindow)shellWindow).GetDialogFrame() ?? throw new InvalidOperationException("Frame not found in shell dialog window.");

        NavigateToPage(frame, pageKey, parameter, shellWindow);

        return shellWindow.ShowDialog();
    }

    /// <summary>
    /// Retrieves the window associated with a specific page key, if it exists.
    /// </summary>
    /// <param name="pageKey">The key identifying the page.</param>
    /// <returns>The window that contains the page, or null if not found.</returns>
    public Window GetWindow(string pageKey)
    {
        return Application.Current.Windows.OfType<Window>().FirstOrDefault(window =>
            window.GetDataContext()?.GetType().FullName == pageKey);
    }

    /// <summary>
    /// Creates a new Metro style window.
    /// </summary>
    /// <returns>A new instance of MetroWindow.</returns>
    private MetroWindow CreateNewWindow()
    {
        return new MetroWindow
        {
            Title = "WPFtemplate",
            Style = Application.Current.FindResource("CustomMetroWindow") as Style
        };
    }

    /// <summary>
    /// Initializes and configures a Frame control in a given window.
    /// </summary>
    /// <param name="window">The window to contain the frame.</param>
    /// <returns>An initialized Frame control.</returns>
    private static Frame InitializeFrame(Window window)
    {
        var frame = new Frame
        {
            Focusable = false,
            NavigationUIVisibility = NavigationUIVisibility.Hidden
        };
        window.SetCurrentValue(ContentControl.ContentProperty, frame);
        return frame;
    }

    /// <summary>
    /// Validates the given page key.
    /// </summary>
    /// <param name="pageKey">The key to validate.</param>
    private void ValidateKey(string pageKey)
    {
        if (string.IsNullOrEmpty(pageKey))
            throw new ArgumentException("PageKey cannot be null or empty.", nameof(pageKey));
    }

    /// <summary>
    /// Navigates to a specified page within a frame and handles window show and navigation events.
    /// </summary>
    /// <param name="frame">The frame to navigate.</param>
    /// <param name="pageKey">The key identifying the page.</param>
    /// <param name="parameter">Optional parameter to pass to the page.</param>
    /// <param name="window">The window containing the frame.</param>
    private void NavigateToPage(Frame frame, string pageKey, object parameter, Window window)
    {
        var page = _pageService.GetPage(pageKey) ?? throw new InvalidOperationException($"Page not found for key: {pageKey}");
        window.Closed += OnWindowClosed;
        window.Show();
        frame.Navigated += OnNavigated;
        frame.Navigate(page, parameter);
    }

    /// <summary>
    /// Handles the Navigated event of the frame.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The NavigationEventArgs containing event data.</param>
    private void OnNavigated(object sender, NavigationEventArgs e)
    {
        if (sender is Frame frame && frame.GetDataContext() is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(e.ExtraData);
        }
    }

    /// <summary>
    /// Handles the Closed event of the window.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The EventArgs containing event data.</param>
    private void OnWindowClosed(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            if (window.Content is Frame frame)
            {
                frame.Navigated -= OnNavigated;
            }

            window.Closed -= OnWindowClosed;
        }
    }
}
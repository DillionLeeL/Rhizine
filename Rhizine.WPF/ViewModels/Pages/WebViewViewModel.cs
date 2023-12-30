using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.WPF.Services.Interfaces;
using System.Windows;
using System.Windows.Input;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class WebViewViewModel : BaseViewModel
{
    private readonly IWebViewService _webViewService;

    private WebView2 _webView;

    public WebViewViewModel(IWebViewService webViewService)
    {
        _webViewService = webViewService;
    }

    [ObservableProperty]
    private Uri _source = new("https://docs.microsoft.com/windows/apps/");

    [ObservableProperty]
    private bool _isShowingFailedMessage;

    [ObservableProperty]
    private bool _isLoading = true;

    [ObservableProperty]
    private Visibility _isLoadingVisibility = Visibility.Visible;

    [ObservableProperty]
    private Visibility _failedMesageVisibility = Visibility.Collapsed;

    public void Initialize(WebView2 webView)
    {
        _webView = webView;
    }

    public void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        IsLoading = false;
        if (e?.IsSuccess == false)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }

        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand]
    private void Reload()
    {
        _webViewService.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        if (_webViewService.CanGoForward)
        {
            _webViewService.GoForward();
        }
    }

    private bool BrowserCanGoForward()
    {
        return _webViewService.CanGoForward;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        if (_webViewService.CanGoBack)
        {
            _webViewService.GoBack();
        }
    }

    private bool BrowserCanGoBack()
    {
        return _webViewService.CanGoBack;
    }

    public void OnNavigatedTo(object parameter)
    {
        _webViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public void OnNavigatedFrom()
    {
        _webViewService.UnregisterEvents();
        _webViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    private void OnNavigationCompleted(object? sender, CoreWebView2WebErrorStatus webErrorStatus)
    {
        IsLoading = false;
        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();

        if (webErrorStatus != default)
        {
            IsShowingFailedMessage = true;
        }
    }

    [RelayCommand]
    private async Task OpenInBrowser()
    {
        if (Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(Source);
        }
    }
}
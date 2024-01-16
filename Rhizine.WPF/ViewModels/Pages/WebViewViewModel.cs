using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.WPF.Services.Interfaces;
using System.Windows;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class WebViewViewModel(IWebViewService webViewService, ILoggingService loggingService) : BaseViewModel
{
    public IWebViewService WebViewService { get; } = webViewService;
    private readonly ILoggingService _loggingService = loggingService;

    [ObservableProperty]
    private Uri _source = new("https://docs.microsoft.com/windows/apps/");

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FailedMesageVisibility))]
    private bool _isShowingFailedMessage;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoadingVisibility))]
    private bool _isLoading;

    public Visibility IsLoadingVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;

    public Visibility FailedMesageVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;

    private bool BrowserCanGoForward => WebViewService.CanGoForward;
    private bool BrowserCanGoBack => WebViewService.CanGoBack;

    [RelayCommand]
    private void Reload()
    {
        IsShowingFailedMessage = false;
        IsLoading = true;
        WebViewService.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        WebViewService.GoForward();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        WebViewService.GoBack();
    }

    public override void OnNavigatedTo(object parameter)
    {
        _loggingService.Log("Navigated to WebViewViewModel.");
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    public override void OnNavigatedFrom()
    {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
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
        if (WebViewService.Source != null)
        {
            await Windows.System.Launcher.LaunchUriAsync(WebViewService.Source);
        }
    }
}
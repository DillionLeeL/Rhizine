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
    #region Fields

    private readonly ILoggingService _loggingService = loggingService;
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsLoadingVisibility))]
    private bool _isLoading;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FailedMesageVisibility))]
    private bool _isShowingFailedMessage;

    [ObservableProperty]
    private Uri _source = new("https://docs.microsoft.com/windows/apps/");

    #endregion Fields

    #region Properties

    public Visibility FailedMesageVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;
    public Visibility IsLoadingVisibility => IsLoading ? Visibility.Visible : Visibility.Collapsed;
    public IWebViewService WebViewService { get; } = webViewService;
    private bool BrowserCanGoBack => WebViewService.CanGoBack;
    private bool BrowserCanGoForward => WebViewService.CanGoForward;

    #endregion Properties

    #region Methods

    public override void OnNavigatedFrom()
    {
        WebViewService.UnregisterEvents();
        WebViewService.NavigationCompleted -= OnNavigationCompleted;
    }

    public override void OnNavigatedTo(object parameter)
    {
        _loggingService.Log("Navigated to WebViewViewModel.");
        WebViewService.NavigationCompleted += OnNavigationCompleted;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack()
    {
        WebViewService.GoBack();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward()
    {
        WebViewService.GoForward();
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

    [RelayCommand]
    private void Reload()
    {
        IsShowingFailedMessage = false;
        IsLoading = true;
        WebViewService.Reload();
    }

    #endregion Methods
}
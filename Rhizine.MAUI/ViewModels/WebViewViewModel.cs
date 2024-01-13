using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels;

public partial class WebViewViewModel(ILoggingService loggingService, INavigationService navigationService, IDialogService dialogService) : BaseViewModel(dialogService, navigationService)
{
    [ObservableProperty]
    private Uri source = new("https://docs.microsoft.com/windows/apps/");

    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private bool hasFailures;

    private readonly ILoggingService _loggingService = loggingService;


    [RelayCommand]
    private async Task OpenInBrowser()
    {
        await Launcher.OpenAsync(Source);
    }

    [RelayCommand]
    private void Reload(WebView webView)
    {
        webView.Reload();
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoForward))]
    private void BrowserForward(WebView webView)
    {
        webView.GoForward();
    }

    private bool BrowserCanGoForward(WebView webView)
    {
        return webView.CanGoForward;
    }

    [RelayCommand(CanExecute = nameof(BrowserCanGoBack))]
    private void BrowserBack(WebView webView)
    {
        webView.GoBack();
    }

    private bool BrowserCanGoBack(WebView webView)
    {
        return webView.CanGoBack;
    }

    [RelayCommand]
    private async Task WebViewNavigated(WebNavigatedEventArgs e)
    {
        IsLoading = false;
        BrowserBackCommand.NotifyCanExecuteChanged();
        BrowserForwardCommand.NotifyCanExecuteChanged();

        if (e.Result != WebNavigationResult.Success)
        {
            HasFailures = true;
            await Shell.Current.DisplayAlert("Navigation failed", e.Result.ToString(), "OK");
        }
    }

    [RelayCommand]
    private void OnRetry(WebView webView)
    {
        HasFailures = false;
        IsLoading = true;
        webView?.Reload();
    }
}
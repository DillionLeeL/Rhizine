using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using Rhizine.WinUI.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;
using Rhizine.Core.Services.Interfaces;

namespace Rhizine.WinUI.Services;

// https://docs.microsoft.com/microsoft-edge/webview2/get-started/winui
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/developer-guide
// https://docs.microsoft.com/microsoft-edge/webview2/concepts/distribution
public class WebViewService : IWebViewService
{
    private WebView2? _webView;

    public Uri? Source => _webView?.Source;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoBack => _webView?.CanGoBack == true;

    [MemberNotNullWhen(true, nameof(_webView))]
    public bool CanGoForward => _webView?.CanGoForward == true;

    public event EventHandler<CoreWebView2WebErrorStatus>? NavigationCompleted;

    [MemberNotNull(nameof(_webView))]
    public void Initialize(WebView2 webView)
    {
        _webView = webView;
        _webView.NavigationCompleted += OnWebViewNavigationCompleted;
    }

    public void GoBack() => _webView?.GoBack();

    public void GoForward() => _webView?.GoForward();

    public void Reload() => _webView?.Reload();

    public void UnregisterEvents()
    {
        if (_webView != null)
        {
            _webView.NavigationCompleted -= OnWebViewNavigationCompleted;
        }
    }

    private void OnWebViewNavigationCompleted(WebView2 sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
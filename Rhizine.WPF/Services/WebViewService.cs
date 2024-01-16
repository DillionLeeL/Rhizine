using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Rhizine.Core.Services.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Rhizine.WPF.Services;

public class WebViewService : IWebViewService<Microsoft.Web.WebView2.Wpf.WebView2, Microsoft.Web.WebView2.Core.CoreWebView2WebErrorStatus>
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

    private void OnWebViewNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs args) => NavigationCompleted?.Invoke(this, args.WebErrorStatus);
}
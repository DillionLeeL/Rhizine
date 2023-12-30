using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace Rhizine.WPF.Services.Interfaces;

public interface IWebViewService
{
    Uri Source { get; }

    bool CanGoBack { get; }

    bool CanGoForward { get; }

    event EventHandler<CoreWebView2WebErrorStatus> NavigationCompleted;

    void Initialize(WebView2 webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}

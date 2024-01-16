namespace Rhizine.Core.Services.Interfaces;

public interface IWebViewService<TWebView, TErrorStatus>
{
    Uri Source { get; }

    bool CanGoBack { get; }

    bool CanGoForward { get; }

    event EventHandler<TErrorStatus> NavigationCompleted;

    void Initialize(TWebView webView);

    void GoBack();

    void GoForward();

    void Reload();

    void UnregisterEvents();
}
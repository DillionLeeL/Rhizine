namespace Rhizine.MAUI.Views;

public partial class WebViewPage : ContentPage
{
    public WebViewPage(WebViewViewModel viewModel)
    {
        InitializeComponent();
        viewModel.Title = "WebView";
        BindingContext = viewModel;
    }
}
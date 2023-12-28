using Microsoft.UI.Xaml.Controls;

using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Views;

public sealed partial class ContentGridPage : Page
{
    public ContentGridViewModel ViewModel
    {
        get;
    }

    public ContentGridPage()
    {
        ViewModel = App.GetService<ContentGridViewModel>();
        InitializeComponent();
    }
}

using Microsoft.UI.Xaml.Controls;

using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>() ?? throw new ArgumentNullException(nameof(ViewModel));
        InitializeComponent();
    }
}
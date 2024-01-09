using Microsoft.UI.Xaml.Controls;
using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Views;

public sealed partial class SettingsPage : Page
{
    public SettingsViewModel ViewModel { get; }

    public SettingsPage()
    {
        ViewModel = App.GetService<SettingsViewModel>() ?? throw new ArgumentNullException(nameof(ViewModel));
        InitializeComponent();
    }
}
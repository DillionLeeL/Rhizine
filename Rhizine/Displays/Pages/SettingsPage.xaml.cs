using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

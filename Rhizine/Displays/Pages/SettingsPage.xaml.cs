using Rhizine.Displays.Pages;
using System.Windows.Controls;

namespace Rhizine.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
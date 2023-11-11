using System.Windows.Controls;

using Rhizine.ViewModels;

namespace Rhizine.Views;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

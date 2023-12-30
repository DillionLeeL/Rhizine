using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Pages;

public partial class SettingsPage : Page
{
    public SettingsPage(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
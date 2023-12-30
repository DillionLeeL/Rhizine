using Rhizine.WPF.ViewModels.Flyouts;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Flyouts;

/// <summary>
/// Interaction logic for SettingFlyout.xaml
/// </summary>
public partial class SettingsFlyout : UserControl
{
    public SettingsFlyout()
    {
        InitializeComponent();
    }

    public SettingsFlyout(SettingsFlyoutViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
using Rhizine.WPF.ViewModels.Flyouts;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Flyouts;
public partial class SimpleFrameFlyout : UserControl
{
    public SimpleFrameFlyout()
    {
        InitializeComponent();
    }

    public SimpleFrameFlyout(SimpleFrameFlyoutViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
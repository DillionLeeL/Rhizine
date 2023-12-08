using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
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
}
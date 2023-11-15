using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
    /// <summary>
    /// TODO: 
    /// </summary>
    public partial class SimpleFrameFlyout : UserControl
    {
        // TODO:
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

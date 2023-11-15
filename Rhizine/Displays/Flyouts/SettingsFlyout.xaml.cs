using System.Windows.Controls;

namespace Rhizine.Displays.Flyouts
{
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
}

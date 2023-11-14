using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

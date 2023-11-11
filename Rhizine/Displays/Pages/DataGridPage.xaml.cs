using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class DataGridPage : Page
{
    public DataGridPage(DataGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

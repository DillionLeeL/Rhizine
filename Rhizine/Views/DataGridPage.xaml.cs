using System.Windows.Controls;

using Rhizine.ViewModels;

namespace Rhizine.Views;

public partial class DataGridPage : Page
{
    public DataGridPage(DataGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

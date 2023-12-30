using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Pages;

public partial class DataGridPage : Page
{
    public DataGridPage(DataGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
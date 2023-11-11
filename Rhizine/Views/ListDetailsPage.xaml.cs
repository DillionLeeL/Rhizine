using System.Windows.Controls;

using Rhizine.ViewModels;

namespace Rhizine.Views;

public partial class ListDetailsPage : Page
{
    public ListDetailsPage(ListDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

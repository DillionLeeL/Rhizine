using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class ListDetailsPage : Page
{
    public ListDetailsPage(ListDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

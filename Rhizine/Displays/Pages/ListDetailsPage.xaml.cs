using Rhizine.Displays.Pages;
using System.Windows.Controls;

namespace Rhizine.Views;

public partial class ListDetailsPage : Page
{
    public ListDetailsPage(ListDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
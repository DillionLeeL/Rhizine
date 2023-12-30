using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Pages;

public partial class ListDetailsPage : Page
{
    public ListDetailsPage(ListDetailsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
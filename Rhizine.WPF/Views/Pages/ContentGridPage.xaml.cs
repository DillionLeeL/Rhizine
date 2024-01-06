using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Pages;

public partial class ContentGridPage : Page
{
    public ContentGridPage(ContentGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
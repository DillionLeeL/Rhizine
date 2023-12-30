using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;
using Rhizine.WPF.Behaviors;

namespace Rhizine.WPF.Views.Pages;
public partial class ContentGridPage : Page
{
    public ContentGridPage(ContentGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
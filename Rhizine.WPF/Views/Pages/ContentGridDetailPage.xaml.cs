using Rhizine.WPF.ViewModels.Pages;
using System.Windows.Controls;

namespace Rhizine.WPF.Views.Pages;

public partial class ContentGridDetailPage : Page
{
    public ContentGridDetailPage(ContentGridDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
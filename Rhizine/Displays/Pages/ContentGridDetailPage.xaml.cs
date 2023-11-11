using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class ContentGridDetailPage : Page
{
    public ContentGridDetailPage(ContentGridDetailViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class ContentGridPage : Page
{
    public ContentGridPage(ContentGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

using Rhizine.Displays.Pages;
using System.Windows.Controls;

namespace Rhizine.Views;

public partial class ContentGridPage : Page
{
    public ContentGridPage(ContentGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
using System.Windows.Controls;

using Rhizine.ViewModels;

namespace Rhizine.Views;

public partial class ContentGridPage : Page
{
    public ContentGridPage(ContentGridViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

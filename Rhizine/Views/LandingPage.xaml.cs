using System.Windows.Controls;

using Rhizine.ViewModels;

namespace Rhizine.Views;

public partial class LandingPage : Page
{
    public LandingPage(LandingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

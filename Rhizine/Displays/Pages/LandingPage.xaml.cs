using System.Windows.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Views;

public partial class LandingPage : Page
{
    public LandingPage(LandingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}

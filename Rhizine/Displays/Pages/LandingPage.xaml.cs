using Rhizine.Displays.Pages;
using System.Windows.Controls;

namespace Rhizine.Views;

public partial class LandingPage : Page
{
    public LandingPage(LandingViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
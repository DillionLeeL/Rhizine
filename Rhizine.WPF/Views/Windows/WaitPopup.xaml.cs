using Rhizine.Core.ViewModels;
using System.Windows;

namespace Rhizine.WPF.Views.Windows;
/// <summary>
/// Interaction logic for WaitPopup.xaml
/// </summary>
public partial class WaitPopup : Window
{
    // Parameterless constructor required for PopupService.CreatePopupView<TViewModel, TView>(TViewModel viewModel)
    public WaitPopup()
    {
        InitializeComponent();
    }
    public WaitPopup(PopupBaseViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

}
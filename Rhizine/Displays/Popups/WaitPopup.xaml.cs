using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Displays.Popups;
using System.Windows;

namespace Rhizine.Displays.Popups;

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
        this.DataContext = viewModel;
    }

}
using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Displays.Popups;
using System.Collections.ObjectModel;
using System.Windows;

namespace WPFBase.Displays.Popups;

public partial class WaitPopupViewModel : PopupBaseViewModel
{
    public ObservableCollection<string> WaitingStates { get; } = new();
    public bool? DialogResult { get; private set; }

    public WaitPopupViewModel()
    {
        IsVisible = true;
    }

    [ObservableProperty]
    private bool _isVisible;

    [ObservableProperty]
    private string _currentStatus;

    public override void Show()
    {
    }

    /*
    [RelayCommand]
    protected override void ClosePopup()
    {
        // Send a message that the popup should be closed.
        base.ClosePopup();
    }
    */

    public void ShowButtons()
    {
        //progressBar.Visibility = Visibility.Collapsed;
        //buttonPanel.Visibility = Visibility.Visible;
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        //Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        //Close();
    }
}
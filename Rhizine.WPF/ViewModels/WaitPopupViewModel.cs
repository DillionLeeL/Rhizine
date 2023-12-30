using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Rhizine.Displays.Popups;

public partial class WaitPopupViewModel : PopupBaseViewModel
{
    private readonly ILoggingService _loggingService;
    public ObservableCollection<string> WaitingStates { get; } = new();
    public bool? DialogResult { get; private set; }

    public WaitPopupViewModel(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        IsVisible = true;
        ProgressBarVisibility = true;
    }

    [ObservableProperty]
    private bool _isVisible;

    [ObservableProperty]
    private string _currentStatus;

    [ObservableProperty]
    private bool _progressBarVisibility;

    public override void Show()
    {
    }
    [RelayCommand]
    public override void Close()
    {
        try
        {
            if (IsClosed)
                return;

            IsClosed = true;
            OnClosing();
            Dispose();
        }
        catch (Exception ex)
        {
            _loggingService?.Log(ex);
        }
    }
    public void AddWaitingState(string state)
    {
        WaitingStates.Add(state);
    }

    public void ShowButtons()
    {
        // TODO: Show buttons
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
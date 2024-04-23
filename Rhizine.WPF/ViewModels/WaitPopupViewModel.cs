using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows;

namespace Rhizine.Displays.Popups;

public partial class WaitPopupViewModel : PopupBaseViewModel
{
    #region Fields

    private readonly ILoggingService _loggingService;
    [ObservableProperty]
    private string _currentStatus;

    [ObservableProperty]
    private bool _isVisible;

    [ObservableProperty]
    private bool _progressBarVisibility;

    #endregion Fields

    #region Constructors

    public WaitPopupViewModel(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        IsVisible = true;
        ProgressBarVisibility = true;
    }

    #endregion Constructors

    #region Properties

    public bool? DialogResult { get; private set; }
    public ObservableCollection<string> WaitingStates { get; } = [];

    #endregion Properties

    #region Methods

    public void AddWaitingState(string state)
    {
        WaitingStates.Add(state);
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

    public override void Show()
    {
    }
    public void ShowButtons()
    {
        // TODO: Show buttons
        //progressBar.Visibility = Visibility.Collapsed;
        //buttonPanel.Visibility = Visibility.Visible;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        //Close();
    }

    private void OkButton_Click(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
        //Close();
    }

    #endregion Methods
}
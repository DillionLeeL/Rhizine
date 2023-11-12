﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Input;
using Rhizine.Displays.Popups;

namespace WPFBase.Displays.Popups;

public partial class WaitPopupViewModel : PopupViewModelBase
{
    public ObservableCollection<string> WaitingStates { get; } = new();
    public bool? DialogResult { get; private set; }
    [ObservableProperty]
    private string _currentStatus;

    public WaitPopupViewModel()
    {
        // If you need to pass parameters or services to the ViewModel, do it here.
    }

    public override void Show()
    {
        // Logic to show the popup.
        // This could involve creating a window, setting its DataContext, etc.
    }

    [RelayCommand]
    protected override void ClosePopup()
    {
        // Send a message that the popup should be closed.
        base.ClosePopup();
    }
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

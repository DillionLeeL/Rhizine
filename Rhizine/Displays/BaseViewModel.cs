using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Displays.Flyouts;
using Rhizine.Displays.Interfaces;
using Rhizine.Services;
using Rhizine.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WPFBase.Services;

namespace WPFBase.Displays;

public partial class BaseViewModel : ObservableObject, INavigationAware
{
    // Replace these with logic
    [ObservableProperty]
    private bool _canExecute;
    [ObservableProperty]
    private bool _canExecuteAsync;
    [ObservableProperty]
    private IFlyoutService _flyoutService;

    [RelayCommand(CanExecute = nameof(CanExecuteCommand))]
    private void MyCommand()
    {
        // Command execution logic here
    }

    private bool CanExecuteCommand()
    {
        return CanExecute;
    }


    [RelayCommand(CanExecute = nameof(CanExecuteAsyncCommand))]
    private async Task MyAsyncCommandAsync()
    {
        // Asynchronous command execution logic here
    }
    private bool CanExecuteAsyncCommand()
    {
        return CanExecuteAsync;
    }
    public IAsyncRelayCommand LoadCommand { get; set; }

    public BaseViewModel()
    {
        LoadCommand = new AsyncRelayCommand(LoadAsync);
    }

    public virtual Task LoadAsync()
    {
        // Override in derived view models to load data
        return Task.CompletedTask;
    }
    public async void OnNavigatedTo(object parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }
    /*
    [RelayCommand]
    private void OpenSimpleFrameFlyout()
    {
        FlyoutService = new FlyoutService();
        //this.FlyoutsControl = new FlyoutsControl();
        //var flyout = new SimpleFrameFlyout(_appConfig.PrivacyStatement);
        //TestFlyout = new SimpleFrameFlyout(_appConfig.PrivacyStatement);
        // when the flyout is closed, remove it from the hosting FlyoutsControl
        
        void ClosingFinishedHandler(object o, RoutedEventArgs args)
        {
            TestFlyout.ClosingFinished -= ClosingFinishedHandler;
            this.FlyoutsControl.Items.Remove(TestFlyout);
        }

        TestFlyout.ClosingFinished += ClosingFinishedHandler;

        //this.FlyoutsControl.Items.Add(TestFlyout);

        TestFlyout.IsOpen = true;
        
        //var test = _appConfig.PrivacyStatement;
        var fly = FlyoutService.CreateFlyout<SimpleFrameFlyoutViewModel>(@"https://www.google.com/");
        Console.WriteLine("Base opensimpleframe");
        FlyoutService.ShowFlyout(fly);
    }
    */
}

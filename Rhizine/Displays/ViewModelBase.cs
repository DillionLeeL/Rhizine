using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WPFBase.Services;

namespace WPFBase.Displays;

public partial class ViewModelBase : ObservableRecipient
{
    // Replace these with logic
    [ObservableProperty]
    private bool _canExecute;
    [ObservableProperty]
    private bool _canExecuteAsync;

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

    public ViewModelBase()
    {
        LoadCommand = new AsyncRelayCommand(LoadAsync);
    }

    public virtual Task LoadAsync()
    {
        // Override in derived view models to load data
        return Task.CompletedTask;
    }
    

}

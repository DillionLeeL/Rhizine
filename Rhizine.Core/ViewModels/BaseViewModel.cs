using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Models.Interfaces;

namespace Rhizine.Core.ViewModels;

public partial class BaseViewModel : ObservableObject, INavigationAware
{
    // Replace these with logic
    [ObservableProperty]
    private bool _canExecute;

    [ObservableProperty]
    private bool _canExecuteAsync;

    // TODO
    public IAsyncRelayCommand LoadCommand { get; set; }

    public BaseViewModel()
    {
        LoadCommand = new AsyncRelayCommand(LoadAsync);
    }

    // TODO: Command execution
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

    public virtual Task LoadAsync()
    {
        // Override in derived view models to load data
        return Task.CompletedTask;
    }

    public virtual void OnNavigatedTo(object parameter)
    {
    }

    public virtual void OnNavigatedFrom()
    {
    }
}
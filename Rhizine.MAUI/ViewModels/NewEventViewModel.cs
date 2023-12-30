using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class NewEventViewModel(IDialogService dialogService, INavigationService<ShellNavigatedEventArgs> navigationService) : BaseViewModel(dialogService, navigationService)
    {
        [ObservableProperty]
        private Event _event = new();

        [RelayCommand]
        private async Task SaveAsync()
        {
            await DialogService.DisplayAlertAsync("Add Event", "Save the event details to a data store.", "OK");
            await NavigationService.GoBackAsync();
        }

        [RelayCommand]
        private Task CancelAsync() => NavigationService.GoBackAsync();
    }
}

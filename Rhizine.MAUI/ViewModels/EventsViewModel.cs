using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class EventsViewModel(IDialogService dialogService, INavigationService<ShellNavigatedEventArgs> navigationService) : BaseViewModel(dialogService, navigationService)
    {
        [RelayCommand]
        private Task AddEventAsync() => NavigationService.NavigateToAsync("newevent");
    }
}

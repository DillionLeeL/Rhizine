using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class EventsViewModel(IDialogService dialogService, INavigationService navigationService) : BaseViewModel
    {
        public IDialogService DialogService => dialogService;

        public INavigationService NavigationService => navigationService;
        [RelayCommand]
        private Task AddEventAsync() => NavigationService.NavigateToAsync("newevent");
    }
}

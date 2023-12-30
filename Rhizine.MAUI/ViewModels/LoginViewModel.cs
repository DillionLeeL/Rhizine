using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class LoginViewModel(IDialogService dialogService, INavigationService<ShellNavigatedEventArgs> navigationService) : BaseViewModel(dialogService, navigationService)
    {
        [RelayCommand]
        private Task LoginAsync() => NavigationService.NavigateToAsync("//home");
    }
}

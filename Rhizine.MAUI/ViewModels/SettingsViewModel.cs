using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class SettingsViewModel(IDialogService dialogService, INavigationService<ShellNavigatedEventArgs> navigationService) : BaseViewModel(dialogService, navigationService)
    {

    }
}

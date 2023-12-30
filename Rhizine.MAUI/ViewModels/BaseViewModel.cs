using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class BaseViewModel(IDialogService dialogService, INavigationService<ShellNavigatedEventArgs> navigationService) : ObservableObject
    {
        public IDialogService DialogService => dialogService;

        public INavigationService<ShellNavigatedEventArgs> NavigationService => navigationService;

        [ObservableProperty]
        private string _title = string.Empty;
    }
}

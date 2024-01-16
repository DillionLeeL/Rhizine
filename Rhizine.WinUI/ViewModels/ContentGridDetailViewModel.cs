using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;

namespace Rhizine.WinUI.ViewModels;

public partial class ContentGridDetailViewModel(ISampleDataService sampleDataService, INavigationService<Microsoft.UI.Xaml.Controls.Frame, Microsoft.UI.Xaml.Navigation.NavigationEventArgs> navigationService) : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService = sampleDataService;
    public readonly INavigationService NavigationService = navigationService;
    [ObservableProperty]
    private SampleOrder? item;

    public async void OnNavigatedTo(object parameter)
    {
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public void OnNavigatedFrom()
    {
    }
}
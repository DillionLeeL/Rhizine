using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;

namespace Rhizine.WinUI.ViewModels;

public partial class ContentGridDetailViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    public readonly INavigationService NavigationService;
    [ObservableProperty]
    private SampleOrder? item;

    public ContentGridDetailViewModel(ISampleDataService sampleDataService, INavigationService<Microsoft.UI.Xaml.Controls.Frame, Microsoft.UI.Xaml.Navigation.NavigationEventArgs> navigationService)
    {
        _sampleDataService = sampleDataService;
        NavigationService = navigationService;
    }

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
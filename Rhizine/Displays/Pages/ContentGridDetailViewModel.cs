using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Displays.Interfaces;
using Rhizine.Models;
using Rhizine.Services.Interfaces;

namespace Rhizine.Displays.Pages;

public partial class ContentGridDetailViewModel : ObservableObject, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;

    [ObservableProperty]
    private SampleOrder _item;

    public ContentGridDetailViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
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
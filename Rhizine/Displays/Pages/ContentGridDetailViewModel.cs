using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using Rhizine.Displays;

namespace Rhizine.Displays.Pages;

public partial class ContentGridDetailViewModel : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private SampleOrder _item;

    public ContentGridDetailViewModel(ISampleDataService sampleDataService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService;
        _loggingService = loggingService;
    }

    public override async Task OnNavigatedTo(object parameter)
    {
        await _loggingService.LogDebugAsync($"ContentGridDetailViewModel.OnNavigatedTo({parameter})");
        if (parameter is long orderID)
        {
            var data = await _sampleDataService.GetContentGridDataAsync();
            Item = data.First(i => i.OrderID == orderID);
        }
    }

    public override async Task OnNavigatedFrom()
    {
    }
}
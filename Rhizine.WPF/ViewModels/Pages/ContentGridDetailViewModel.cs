using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class ContentGridDetailViewModel(ISampleDataService sampleDataService, ILoggingService loggingService) : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService = sampleDataService;
    private readonly ILoggingService _loggingService = loggingService;

    [ObservableProperty]
    private SampleOrder _item;

    public override async void OnNavigatedTo(object parameter)
    {
        try
        {
            if (parameter is long orderID)
            {
                var data = await _sampleDataService.GetContentGridDataAsync();
                Item = data.First(i => i.OrderID == orderID);
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, "Error while navigating to Content Grid Detail");
        }
    }
}
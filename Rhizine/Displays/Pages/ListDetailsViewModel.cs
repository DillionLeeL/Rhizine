using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Collections.ObjectModel;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public class ListDetailsViewModel : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;
    private SampleOrder _selected;

    public SampleOrder Selected
    {
        get { return _selected; }
        set { SetProperty(ref _selected, value); }
    }

    public ObservableCollection<SampleOrder> SampleItems { get; } = new ObservableCollection<SampleOrder>();

    public ListDetailsViewModel(ISampleDataService sampleDataService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService;
        _loggingService = loggingService;
    }

    public override async Task OnNavigatedTo(object parameter)
    {
        _loggingService.LogInformation("Navigated to List Details");
        SampleItems.Clear();

        var data = await _sampleDataService.GetListDetailsDataAsync();

        foreach (var item in data)
        {
            SampleItems.Add(item);
        }

        Selected = SampleItems.First();
    }

}
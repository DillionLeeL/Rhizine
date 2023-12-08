using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Collections.ObjectModel;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public class DataGridViewModel : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService;

    public ObservableCollection<SampleOrder> Source { get; } = [];

    public DataGridViewModel(ISampleDataService sampleDataService)
    {
        _sampleDataService = sampleDataService;
    }

    public override async Task OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Replace this with your actual data
        var data = await _sampleDataService.GetGridDataAsync();

        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public override async Task OnNavigatedFrom()
    {
    }
}
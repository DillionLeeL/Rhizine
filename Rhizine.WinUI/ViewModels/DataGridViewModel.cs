using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Rhizine.WinUI.ViewModels;

public partial class DataGridViewModel : ObservableRecipient, INavigationAware
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;

    public ObservableCollection<SampleOrder> Source { get; } = [];

    public DataGridViewModel(ISampleDataService sampleDataService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService ?? throw new ArgumentNullException(nameof(sampleDataService));
        _loggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
        _loggingService.Log("Constructed DataGridViewModel.");
    }

    public async void OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // TODO: Replace with real data.
        var data = await _sampleDataService.GetGridDataAsync() ?? [];

        await _loggingService.LogAsync($"Loaded {data.Count()} items.");
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    public void OnNavigatedFrom()
    {
        _loggingService.Log("Navigated from DataGridViewModel.");
    }
}
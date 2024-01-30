using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Serilog.Core;
using System.Collections.ObjectModel;

namespace Rhizine.MAUI.ViewModels;

public partial class ListDetailsViewModel : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;

    public ListDetailsViewModel(ISampleDataService sampleDataService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService;
        _loggingService = loggingService;
        LoadDataAsync().ConfigureAwait(false);
    }

    private SampleOrder? selected;

    public SampleOrder Selected
    {
        get => selected ??= SampleItems.First();
        set => SetProperty(ref selected, value);
    }
    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    public ObservableCollection<SampleOrder> _sampleItems = [];

    public async Task LoadDataAsync()
    {
        try
        {
            SampleItems = new ObservableCollection<SampleOrder>(await _sampleDataService.GetListDetailsDataAsync());
            await _loggingService.LogInformationAsync($"Loaded List Details with {SampleItems.Count} items");
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, "Error while loading List Details");
        }
    }
}
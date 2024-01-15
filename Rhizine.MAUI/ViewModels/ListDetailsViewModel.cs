using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Models.Interfaces;
using Rhizine.Core.Services;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Rhizine.MAUI.ViewModels;

public partial class ListDetailsViewModel(ISampleDataService sampleDataService, ILoggingService loggingService) : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService = sampleDataService;
    private readonly ILoggingService _loggingService = loggingService;

    [ObservableProperty]
    private SampleOrder? selected;
    [ObservableProperty]
    bool isRefreshing;

    [ObservableProperty]
    public ObservableCollection<SampleOrder> _sampleItems = new ObservableCollection<SampleOrder>();

    public async Task LoadDataAsync()
    {
        SampleItems = new ObservableCollection<SampleOrder>(await _sampleDataService.GetListDetailsDataAsync());
    }
}
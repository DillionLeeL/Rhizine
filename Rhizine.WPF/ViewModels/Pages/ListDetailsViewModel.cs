using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class ListDetailsViewModel : BaseViewModel
{
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private SampleOrder _selected;

    public ObservableCollection<SampleOrder> SampleItems { get; } = new();

    public ListDetailsViewModel(ISampleDataService sampleDataService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService;
        _loggingService = loggingService;
    }

    public override async void OnNavigatedTo(object parameter)
    {
        try
        {
            await _loggingService.LogInformationAsync("Navigated to List Details");
            SampleItems.Clear();

            var data = await _sampleDataService.GetListDetailsDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            Selected = SampleItems.First();
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, "Error while navigating to List Details");
        }

    }

}
using CommunityToolkit.Mvvm.ComponentModel;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class ListDetailsViewModel(ISampleDataService sampleDataService, ILoggingService loggingService) : BaseViewModel
{
    #region Fields

    private readonly ILoggingService _loggingService = loggingService;
    private readonly ISampleDataService _sampleDataService = sampleDataService;
    [ObservableProperty]
    private SampleOrder _selected;

    #endregion Fields

    #region Properties

    public ObservableCollection<SampleOrder> SampleItems { get; } = [];

    #endregion Properties

    #region Methods

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

    #endregion Methods
}
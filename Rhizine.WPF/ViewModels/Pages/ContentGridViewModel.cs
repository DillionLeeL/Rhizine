using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class ContentGridViewModel(ISampleDataService sampleDataService, INavigationService navigationService, ILoggingService loggingService) : BaseViewModel
{
    private readonly INavigationService _navigationService = navigationService;
    private readonly ISampleDataService _sampleDataService = sampleDataService;
    private readonly ILoggingService _loggingService = loggingService;

    public ObservableCollection<SampleOrder> Source { get; } = [];

    public override async void OnNavigatedTo(object parameter)
    {
        try
        {
            Source.Clear();

            // Replace this with your actual data
            var data = await _sampleDataService.GetContentGridDataAsync();
            foreach (var item in data)
            {
                Source.Add(item);
            }
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex, "Error while navigating to Content Grid");
        }
    }

    [RelayCommand]
    private void NavigateToDetail(SampleOrder order)
    {
        if (order != null)
        {
            _navigationService.NavigateToAsync(typeof(ContentGridDetailViewModel).FullName, order.OrderID);
        }
    }
}
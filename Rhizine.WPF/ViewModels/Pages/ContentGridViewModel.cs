using CommunityToolkit.Mvvm.Input;
using Rhizine.Core.Models;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.Displays.Pages;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Navigation;

namespace Rhizine.WPF.ViewModels.Pages;

public partial class ContentGridViewModel : BaseViewModel
{
    private readonly INavigationService<NavigationEventArgs> _navigationService;
    private readonly ISampleDataService _sampleDataService;
    private readonly ILoggingService _loggingService;

    public ObservableCollection<SampleOrder> Source { get; } = [];

    public ContentGridViewModel(ISampleDataService sampleDataService, INavigationService<NavigationEventArgs> navigationService, ILoggingService loggingService)
    {
        _sampleDataService = sampleDataService;
        _navigationService = navigationService;
        _loggingService = loggingService;
    }

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
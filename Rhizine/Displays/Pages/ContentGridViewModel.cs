using CommunityToolkit.Mvvm.Input;
using Rhizine.Models;
using Rhizine.Services.Interfaces;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public class ContentGridViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ISampleDataService _sampleDataService;
    private ICommand _navigateToDetailCommand;

    public ICommand NavigateToDetailCommand => _navigateToDetailCommand ??= new RelayCommand<SampleOrder>(NavigateToDetail);

    public ObservableCollection<SampleOrder> Source { get; } = [];

    public ContentGridViewModel(ISampleDataService sampleDataService, INavigationService navigationService)
    {
        _sampleDataService = sampleDataService;
        _navigationService = navigationService;
    }

    public override async Task OnNavigatedTo(object parameter)
    {
        Source.Clear();

        // Replace this with your actual data
        var data = await _sampleDataService.GetContentGridDataAsync();
        foreach (var item in data)
        {
            Source.Add(item);
        }
    }

    private void NavigateToDetail(SampleOrder order)
    {
        _navigationService.NavigateTo(typeof(ContentGridDetailViewModel).FullName, order.OrderID);
    }
}
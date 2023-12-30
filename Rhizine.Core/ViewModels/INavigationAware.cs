namespace Rhizine.Core.Models.Interfaces;

public interface INavigationAware
{
    // TODO: async navigation, not void, with default implementation
    void OnNavigatedTo(object parameter);

    void OnNavigatedFrom();
}
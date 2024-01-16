using Rhizine.Core.Services;
using Rhizine.Core.Services.Interfaces;
using System.Text.Json;

namespace Rhizine.MAUI.Services;

// TODO: non-shell navigation
public partial class NavigationService : INavigationService
{
    private readonly ILoggingService _loggingService;

    public Shell NavigationSource => Shell.Current;

    public bool CanGoBack => NavigationSource.Navigation?.NavigationStack?.Count > 1;

    public event EventHandler<ShellNavigatedEventArgs> Navigated;

    public NavigationService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        Shell.Current.Navigated += (sender, args) => OnNavigated(args);
    }

    public void Initialize(object? parameter = null)
    {
        if (Shell.Current is null)
        {
            throw new NotSupportedException("Navigation is currently supported only with a Shell-enabled application.");
        }
    }

    public async Task NavigateToAsync(string route, object? parameter = null, bool clearNavigation = false)
    {
        try
        {
            var navigationParameter = "";
            if (parameter != null)
            {
                // TODO: maui page query parameter
                //var queryPropertyName = "data"; // This should match the QueryProperty name in the target page
                //navigationParameter = $"?{queryPropertyName}={Uri.EscapeDataString(JsonSerializer.Serialize(parameter))}";
            }

            var fullRoute = clearNavigation ? $"//{route}{navigationParameter}" : $"{route}{navigationParameter}";

            await Shell.Current.GoToAsync(fullRoute);
        }
        catch (Exception ex)
        {
            await _loggingService.LogErrorAsync(ex);
        }
    }

    public async Task GoBackAsync()
    {
        await NavigationSource.GoToAsync("..");
    }

    protected virtual void OnNavigated(ShellNavigatedEventArgs args)
    {
        Navigated?.Invoke(this, args);
    }

    //TODO
    public void Initialize(Shell source)
    {
        throw new NotImplementedException();
    }

    public bool NavigateTo(string pageKey, object? parameter = null, bool clearNavigation = false)
    {
        throw new NotImplementedException();
    }

    public bool GoBack()
    {
        if (CanGoBack)
        {
            _ = GoBackAsync();
            return true;
        }
        return false;
    }
}

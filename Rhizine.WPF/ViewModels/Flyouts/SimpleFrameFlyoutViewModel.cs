using CommunityToolkit.Mvvm.ComponentModel;
using MahApps.Metro.Controls;
using Rhizine.Core.Services.Interfaces;
using System.Windows.Controls;

namespace Rhizine.WPF.ViewModels.Flyouts;

public partial class SimpleFrameFlyoutViewModel : FlyoutBaseViewModel
{
    #region Fields

    private readonly ILoggingService _loggingService;

    [ObservableProperty]
    private Page _currentPage;

    [ObservableProperty]
    private Uri _currentUri;

    #endregion Fields

    #region Constructors

    public SimpleFrameFlyoutViewModel()
    { }

    public SimpleFrameFlyoutViewModel(Uri source)
    {
        CurrentUri = source;
        Header = "frame";
        Position = Position.Left;
    }

    public SimpleFrameFlyoutViewModel(Uri source, ILoggingService loggingService)
    {
        CurrentUri = source;
        Header = "frame";
        Position = Position.Left;
        _loggingService = loggingService;
        _loggingService.LogDebug($"SimpleFrameFlyoutViewModel constructor with {source}");
    }

    public SimpleFrameFlyoutViewModel(Page page, ILoggingService loggingService)
    {
        CurrentPage = page;
        Header = "frame";
        Position = Position.Left;
        _loggingService = loggingService;
        _loggingService.LogInformation($"SimpleFrameFlyoutViewModel constructor with {page}");
    }

    #endregion Constructors
}
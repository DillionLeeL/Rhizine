using Rhizine.Displays.Popups;
using Rhizine.Helpers;
using System.Windows;

namespace Rhizine.Services.Interfaces;

public interface IPopupService
{
    TView CreatePopupView<TViewModel, TView>(TViewModel viewModel)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new();

    TViewModel CreatePopupViewModel<TViewModel>(params object[] services)
        where TViewModel : PopupBaseViewModel;

    Task ShowPopupAsync<TViewModel, TView>(TViewModel viewModel = null, TView popupView = null)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new();

    Task<TResult> ShowPopupAsync<TViewModel, TView, TResult>(TViewModel viewModel = null)
        where TViewModel : PopupBaseViewModel, IResultProvider<TResult>
        where TView : Window, new();

    Task ClosePopupAsync(PopupBaseViewModel viewModel);

    void ApplyAnimation(Window window, bool opening);
}
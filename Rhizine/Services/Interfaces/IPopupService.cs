using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.VisualStudio.Shell;
using Rhizine.Displays.Popups;
using Rhizine.Helpers;
using Rhizine.Messages;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media.Animation;
using WPFBase.Displays.Popups;

namespace WPFBase.Services;

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
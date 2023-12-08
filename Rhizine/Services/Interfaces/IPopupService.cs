using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.VisualStudio.Shell;
using Rhizine.Displays.Popups;
using Rhizine.Messages;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media.Animation;
using WPFBase.Displays.Popups;

namespace WPFBase.Services;

public interface IPopupService
{
    Task ShowPopupAsync(PopupBaseViewModel viewModel, Window popupView);


    Task AddWaitingStateAsync(WaitPopupViewModel viewModel, string state);

    Task ClosePopupAsync(PopupBaseViewModel viewModel);


    void ApplyAnimation(Window window, bool opening);


}
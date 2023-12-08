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

public class PopupService
{
    private readonly ConcurrentDictionary<PopupBaseViewModel, Window> _openPopups = new();

    public bool DialogResult { get; }

    public PopupService()
    {
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, (r, m) => ClosePopupAsync(m.ViewModel).ConfigureAwait(false));
        WeakReferenceMessenger.Default.Register<PopupClosingMessage>(this, (recipient, message) =>
        {
            if (_openPopups.TryRemove(message.Value, out Window window))
            {
                window.Close();
            }
        });
    }

    ~PopupService()
    {
        UnregisterMessages();
    }
    private Window CreatePopupView<TViewModel>(TViewModel viewModel) where TViewModel : PopupBaseViewModel
    {
        var popupView = new Window
        {
            DataContext = viewModel,
            // Other initialization logic...
        };
        return popupView;
    }
    public async Task ShowPopupAsync(PopupBaseViewModel viewModel, Window popupView)
    {
        if (popupView == null || viewModel == null)
            throw new ArgumentNullException();

        if (!_openPopups.TryAdd(viewModel, popupView))
            throw new InvalidOperationException("Popup already shown for this ViewModel.");

        popupView.Closed += (s, e) => _openPopups.TryRemove(viewModel, out _);

        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        popupView.Show();
    }

    public async Task ShowPopupAsync<TViewModel>(TViewModel viewModel) where TViewModel : PopupBaseViewModel
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var popupView = CreatePopupView(viewModel);
        if (!_openPopups.TryAdd(viewModel, popupView))
            throw new InvalidOperationException("Popup already shown for this ViewModel.");

        popupView.Closed += (s, e) => _openPopups.TryRemove(viewModel, out _);

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            ApplyAnimation(popupView, true);
            popupView.Show();
        });
    }
    public async Task<TResult> ShowPopupAsync<TViewModel, TResult>(TViewModel viewModel) where TViewModel : PopupBaseViewModel, IResultProvider<TResult>
    {
        ArgumentNullException.ThrowIfNull(viewModel);

        var popupView = CreatePopupView(viewModel);
        if (!_openPopups.TryAdd(viewModel, popupView))
            throw new InvalidOperationException("Popup already shown for this ViewModel.");

        TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();

        EventHandler handler = null;
        handler = (sender, args) =>
        {
            _openPopups.TryRemove(viewModel, out _);
            tcs.TrySetResult(viewModel.Result);
            viewModel.Closing -= handler;
        };

        viewModel.Closing += handler;
        popupView.Closed += (s, e) => viewModel.Close();

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            ApplyAnimation(popupView, true);
            popupView.Show();
        });

        return await tcs.Task;
    }

    private async Task ClosePopupAsync(PopupBaseViewModel viewModel)
    {
        if (_openPopups.TryRemove(viewModel, out var popupView))
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            WeakReferenceMessenger.Default.Send(new ClosePopupMessage(viewModel));
            popupView.Close();
        }
    }

    private static void ApplyAnimation(Window window, bool opening)
    {
        var duration = new Duration(TimeSpan.FromMilliseconds(200));
        var anim = new DoubleAnimation(opening ? 0 : 1, opening ? 1 : 0, duration)
        {
            FillBehavior = FillBehavior.Stop
        };
        anim.Completed += (s, e) =>
        {
            if (!opening)
            {
                window.SetCurrentValue(UIElement.OpacityProperty, 1.0); // Reset opacity after closing animation
            }
        };
        window.BeginAnimation(UIElement.OpacityProperty, anim);
    }

    public void UnregisterMessages()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}

public interface IResultProvider<TResult>
{
    TResult Result { get; }
}
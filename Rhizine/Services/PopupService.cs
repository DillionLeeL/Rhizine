using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Threading;
using Rhizine.Displays.Popups;
using Rhizine.Messages;
using Rhizine.Services.Interfaces;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media.Animation;
using Rhizine.Helpers;

namespace WPFBase.Services;

public class PopupService : IPopupService
{
    private readonly ConcurrentDictionary<PopupBaseViewModel, Window> _openPopups = new();
    private readonly ILoggingService _loggingService;

    public bool DialogResult { get; }

    public PopupService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, handler: (r, m) => ClosePopupAsync(m.ViewModel).ConfigureAwait(false));
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

    public TView CreatePopupView<TViewModel, TView>(TViewModel viewModel)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new()
    {
        return new TView()
        {
            DataContext = viewModel,
            // TODO: Other initialization logic...
        };
    }

    public TViewModel CreatePopupViewModel<TViewModel>(params object[] services)
        where TViewModel : PopupBaseViewModel
    {
        try
        {
            return (TViewModel)Activator.CreateInstance(typeof(TViewModel), services);
        }
        catch (MissingMethodException)
        {
            throw new InvalidOperationException("ViewModel must have a parameterless constructor.");
        }
        catch (Exception ex)
        {
            _loggingService.Log(ex);
            throw;
        }
    }

    public async Task ShowPopupAsync<TViewModel, TView>(TViewModel viewModel = null, TView popupView = null)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new()
    {
        viewModel ??= CreatePopupViewModel<TViewModel>();
        popupView ??= CreatePopupView<TViewModel, TView>(viewModel);

        if (!_openPopups.TryAdd(viewModel, popupView))
            throw new InvalidOperationException("Popup already shown for this ViewModel.");

        popupView.Closed += (s, e) => _openPopups.TryRemove(viewModel, out _);

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            ApplyAnimation(popupView, true);
            popupView.Show();
        });
    }

    // TODO: Write a popup base view
    public async Task<TResult> ShowPopupAsync<TViewModel, TView, TResult>(TViewModel viewModel = null)
        where TViewModel : PopupBaseViewModel, IResultProvider<TResult>
        where TView : Window, new()
    {
        viewModel ??= CreatePopupViewModel<TViewModel>();
        var popupView = CreatePopupView<TViewModel, TView>(viewModel);

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

    public async Task ClosePopupAsync(PopupBaseViewModel viewModel)
    {
        if (_openPopups.TryRemove(viewModel, out var popupView))
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                WeakReferenceMessenger.Default.Send(new ClosePopupMessage(viewModel));
                popupView.Close();
            });
        }
    }

    public void ApplyAnimation(Window window, bool opening)
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
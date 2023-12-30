using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Core.Helpers;
using Rhizine.Core.Services.Interfaces;
using Rhizine.Core.ViewModels;
using Rhizine.WPF.Messages;
using Rhizine.WPF.Services.Interfaces;
using System.Collections.Concurrent;
using System.Windows;
using System.Windows.Media.Animation;

namespace Rhizine.WPF.Services;

public class PopupService : IPopupService, IDisposable
{
    private const int AnimationDuration = 200; // in milliseconds
    private readonly ConcurrentDictionary<PopupBaseViewModel, Window> _openPopups = new();
    private readonly ILoggingService _loggingService;

    public bool DialogResult { get; }

    /// <summary>
    /// Initializes a new instance of the PopupService with a logging service.
    /// </summary>
    /// <param name="loggingService">Service for logging errors and information.</param>
    public PopupService(ILoggingService loggingService)
    {
        _loggingService = loggingService;
        RegisterMessages();
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        UnregisterMessages();
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Registers message handlers for handling popup close events using WeakReferenceMessenger to avoid memory leaks.
    /// </summary>
    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, (r, m) =>
            ClosePopupAsync(m.ViewModel).ConfigureAwait(false));
        WeakReferenceMessenger.Default.Register<PopupClosingMessage>(this, PopupClosedHandler);
    }

    /// <summary>
    /// Creates a new popup view of the specified type, setting the provided view model as its DataContext.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model.</typeparam>
    /// <typeparam name="TView">Type of the popup view. Must have a parameterless constructor.</typeparam>
    /// <param name="viewModel">ViewModel to be set as the DataContext of the popup view.</param>
    /// <returns>The created popup view.</returns>
    public TView CreatePopupView<TViewModel, TView>(TViewModel viewModel)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new()
    {
        // Option for non-parameterless constructor:
        // return ActivatorUtilities.CreateInstance<TView>(Ioc.Default.GetRequiredService<IServiceProvider>(), viewModel);
        return new TView()
        {
            DataContext = viewModel,
            // Other initialization logic...
        };
    }

    /// <summary>
    /// Creates a new instance of the specified popup view model type, optionally using DI services.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model to create.</typeparam>
    /// <param name="services">Optional DI services to be used in the view model's constructor.</param>
    /// <returns>The created view model instance.</returns>
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

    /// <summary>
    /// Asynchronously displays a popup for the specified view model and view.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model.</typeparam>
    /// <typeparam name="TView">Type of the popup view.</typeparam>
    /// <param name="viewModel">Optional view model. If null, a new instance is created.</param>
    /// <param name="popupView">Optional view. If null, a new instance is created.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ShowPopupAsync<TViewModel, TView>(TViewModel viewModel = null, TView popupView = null)
        where TViewModel : PopupBaseViewModel
        where TView : Window, new()
    {
        viewModel ??= (popupView != null ? popupView.DataContext as TViewModel : CreatePopupViewModel<TViewModel>());
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

    /// <summary>
    /// Asynchronously displays a popup and returns a result when the popup is closed.
    /// </summary>
    /// <typeparam name="TViewModel">Type of the view model providing the result.</typeparam>
    /// <typeparam name="TView">Type of the popup view.</typeparam>
    /// <typeparam name="TResult">Type of the result returned when the popup closes.</typeparam>
    /// <param name="viewModel">Optional view model. If null, a new instance is created.</param>
    /// <returns>The result from the popup when it closes.</returns>
    public async Task<TResult> ShowPopupAsync<TViewModel, TView, TResult>(TViewModel viewModel = null)
        where TViewModel : PopupBaseViewModel, IResultProvider<TResult>
        where TView : Window, new()
    {
        viewModel ??= CreatePopupViewModel<TViewModel>();
        var popupView = CreatePopupView<TViewModel, TView>(viewModel);

        if (!_openPopups.TryAdd(viewModel, popupView))
            throw new InvalidOperationException("Popup already shown for this ViewModel.");

        TaskCompletionSource<TResult> tcs = new TaskCompletionSource<TResult>();

        void Handler(object sender, EventArgs args)
        {
            _openPopups.TryRemove(viewModel, out _);
            tcs.TrySetResult(viewModel.Result);
            viewModel.Closing -= Handler;
        }

        viewModel.Closing += Handler;
        popupView.Closed += (s, e) => viewModel.Close();

        await Application.Current.Dispatcher.InvokeAsync(() =>
        {
            ApplyAnimation(popupView, true);
            popupView.Show();
        });

        return await tcs.Task;
    }

    /// <summary>
    /// Asynchronously closes a popup associated with the specified view model.
    /// </summary>
    /// <param name="viewModel">ViewModel associated with the popup to close.</param>
    /// <returns>A Task representing the asynchronous operation.</returns>
    public async Task ClosePopupAsync(PopupBaseViewModel viewModel)
    {
        if (_openPopups.TryRemove(viewModel, out var popupView))
        {
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                WeakReferenceMessenger.Default.Send(new ClosePopupMessage(viewModel));

                if (popupView is Window window)
                    ApplyAnimation(window, false); // Close animation for popups

                popupView?.Close();
            });
        }
    }

    /// <summary>
    /// Handler for PopupClosingMessage. Removes the popup associated with the view model from the open popups collection.
    /// </summary>
    /// <param name="recipient">The recipient of the message.</param>
    /// <param name="message">The message containing the view model associated with the popup to be closed.</param>
    private void PopupClosedHandler(object recipient, PopupClosingMessage message)
    {
        if (_openPopups.TryRemove(message.Value, out Window view))
            return; // Ignore messages for popups that have already been closed

        ApplyAnimation(view, false);
    }

    /// <summary>
    /// Applies an opacity animation to the specified window for opening or closing effects.
    /// </summary>
    /// <param name="window">Window on which to apply the animation.</param>
    /// <param name="opening">True to apply opening animation, false for closing.</param>
    public void ApplyAnimation(Window window, bool opening)
    {
        var duration = new Duration(TimeSpan.FromMilliseconds(AnimationDuration));
        var anim = new DoubleAnimation(opening ? 0 : 1, opening ? 1 : 0, duration);

        window?.BeginAnimation(UIElement.OpacityProperty, anim);
    }

    /// <summary>
    /// Unregisters all messages associated with this service.
    /// </summary>
    public void UnregisterMessages()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}
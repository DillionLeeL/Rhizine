using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Displays.Popups;
using System.Windows;
using System.Windows.Media.Animation;
using WPFBase.Displays.Popups;

namespace WPFBase.Services;

public class PopupService
{
    // Dictionary to keep track of open popups and their associated ViewModels
    private readonly Dictionary<PopupBaseViewModel, Window> _openPopups = new();
    private WaitPopup _popup;
    private readonly object _lock = new();
    public bool DialogResult { get; private set; }

    public PopupService()
    {
        // Register to receive the ClosePopupMessage
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, (r, m) => ClosePopup(m.Popup));
    }
    ~PopupService()
    {
        UnregisterMessages();
    }
    public void ShowPopup()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            lock (_lock)
            {
                if (_popup != null) return; // Popup is already shown

                _popup = new WaitPopup();
                _popup.Closed += (s, e) => _popup = null;
                ApplyAnimation(_popup, true);
                _popup.Show();
            }
        });
    }
    public void ShowPopup(PopupBaseViewModel viewModel, Window popupView)
    {
        // Associate the ViewModel with its View.
        _openPopups[viewModel] = popupView;

        // Show the popup.
        popupView.Show();
    }
    public void AddWaitingState(string state)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            if (_popup?.DataContext is WaitPopupViewModel viewModel)
            {
                viewModel.WaitingStates.Add(state);
            }
        });
    }
    private void ClosePopup(PopupBaseViewModel viewModel)
    {
        // Check if the ViewModel has an associated View.
        if (_openPopups.TryGetValue(viewModel, out var popupView))
        {
            // Close the popup on the UI thread.
            Application.Current.Dispatcher.Invoke(() =>
            {
                popupView.Close();
            });

            // Remove the ViewModel from the dictionary.
            _openPopups.Remove(viewModel);
        }
    }
    public void ClosePopup()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            _popup?.Close();
        });
    }
    private void ApplyAnimation(Window window, bool opening)
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
                window.Opacity = 1; // Reset opacity after closing animation
            }
        };
        window.BeginAnimation(UIElement.OpacityProperty, anim);
    }
    // Call this method to clean up when the application is closing or the service is no longer needed.
    public void UnregisterMessages()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
    }
}


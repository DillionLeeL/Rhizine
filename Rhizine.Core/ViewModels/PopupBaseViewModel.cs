using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Core.Messages;

namespace Rhizine.Core.ViewModels;

public abstract partial class PopupBaseViewModel : ObservableRecipient, IDisposable
{
    protected PopupBaseViewModel()
    {
        IsActive = true;
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, (r, m) =>
        {
            if (m.Value == this && !_isClosed)
            {
                Close();
            }
        });
    }
    [ObservableProperty]
    private bool _isClosed;
    // Method to show the popup. Must be implemented by derived classes.
    [RelayCommand]
    public abstract void Show();

    // Method to hide the popup. Can be overridden in derived classes.
    [RelayCommand]
    public virtual void Hide()
    {
        if (IsClosed)
            return;
    }

    public virtual void Close()
    {
        if (IsClosed)
            return;

        IsClosed = true;
        OnClosing();
        Dispose();
    }
    // Event to signal when the popup is closing.
    public event EventHandler Closing;
    // Method called when the popup is closing. Can be overridden in derived classes.
    protected virtual void OnClosing()
    {
        WeakReferenceMessenger.Default.Send(new PopupClosingMessage(this));
    }

    public void Dispose()
    {
        WeakReferenceMessenger.Default.UnregisterAll(this);
        GC.SuppressFinalize(this);
    }
}

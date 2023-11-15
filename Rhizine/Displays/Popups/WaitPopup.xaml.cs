using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Displays.Popups;
using System.Windows;

namespace WPFBase.Displays.Popups;

/// <summary>
/// Interaction logic for WaitPopup.xaml
/// </summary>
public partial class WaitPopup : Window
{
    public WaitPopup()
    {
        InitializeComponent();
        //DataContext = Ioc.Default.GetService<WaitPopupViewModel>();
        this.DataContext = new WaitPopupViewModel();
        //this.Show();
        // Register to receive the ClosePopupMessage
        WeakReferenceMessenger.Default.Register<ClosePopupMessage>(this, (r, m) => Close());
    }
    protected override void OnClosed(EventArgs e)
    {
        // Unregister the message when the window is closed to prevent memory leaks
        WeakReferenceMessenger.Default.Unregister<ClosePopupMessage>(this);
        base.OnClosed(e);
    }
}

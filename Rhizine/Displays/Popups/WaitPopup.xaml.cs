using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using Rhizine.Displays.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Rhizine.Displays.Flyouts;
using Rhizine.Services;
using WPFBase.Displays;

namespace Rhizine.Displays.Pages;

public partial class LandingViewModel : BaseViewModel
{
    public LandingViewModel()
    {
        
    }

    [RelayCommand]
    private void OpenSimpleFrameFlyout()
    {
        Console.WriteLine("landing opensimpleframe");
        base.OpenSimpleFrameFlyoutCommand.Execute(null);
    }

    public void OpenFlyout_Click(object sender, System.Windows.RoutedEventArgs e)
    {
        Console.WriteLine("click opensimpleframe");
        base.OpenSimpleFrameFlyoutCommand.Execute(null);
    }
}

using MahApps.Metro.Controls;
using Rhizine.Displays.Pages;

namespace Rhizine.Displays.Flyouts
{
    public class SettingsFlyoutViewModel : FlyoutBaseViewModel
    {
        public SettingsFlyoutViewModel()
        {
            this.Header = "settings";
            this.Position = Position.Left;
        }
    }
}
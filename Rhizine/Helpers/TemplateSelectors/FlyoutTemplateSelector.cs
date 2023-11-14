using Rhizine.Displays.Flyouts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Rhizine.Helpers.TemplateSelectors
{
    public class FlyoutTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SettingsFlyoutTemplate { get; set; }
        public DataTemplate SimpleFrameFlyoutTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item switch
            {
                SettingsFlyoutViewModel => SettingsFlyoutTemplate,
                SimpleFrameFlyoutViewModel => SimpleFrameFlyoutTemplate,
                _ => base.SelectTemplate(item, container),
            };
        }
    }
}

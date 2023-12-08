using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Displays.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhizine.Messages
{
    public class PopupClosingMessage : ValueChangedMessage<PopupBaseViewModel>
    {
        public PopupClosingMessage(PopupBaseViewModel viewModel) : base(viewModel)
        {
        }
    }
}

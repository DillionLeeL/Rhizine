using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Displays.Popups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhizine.Messages
{
    public class ClosePopupMessage : ValueChangedMessage<PopupBaseViewModel>
    {
        public PopupBaseViewModel ViewModel { get; }
        public ClosePopupMessage(PopupBaseViewModel viewModel) : base(viewModel)
        {
            ViewModel = viewModel;
        }
    }
}

using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Core.ViewModels;

namespace Rhizine.Core.Messages
{
    public class PopupClosingMessage : ValueChangedMessage<PopupBaseViewModel>
    {
        public PopupClosingMessage(PopupBaseViewModel viewModel) : base(viewModel)
        {
        }
    }
}
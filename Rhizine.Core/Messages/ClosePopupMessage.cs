using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Core.ViewModels;

namespace Rhizine.Core.Messages
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
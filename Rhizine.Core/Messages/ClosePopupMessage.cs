using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Core.ViewModels;

namespace Rhizine.Core.Messages
{
    public class ClosePopupMessage(PopupBaseViewModel viewModel) : ValueChangedMessage<PopupBaseViewModel>(viewModel)
    {
        public PopupBaseViewModel ViewModel { get; } = viewModel;
    }
}
using CommunityToolkit.Mvvm.Messaging.Messages;
using Rhizine.Core.ViewModels;

namespace Rhizine.WPF.Messages;

public class PopupClosingMessage(PopupBaseViewModel viewModel) : ValueChangedMessage<PopupBaseViewModel>(viewModel)
{
}
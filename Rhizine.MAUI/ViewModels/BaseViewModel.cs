using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;
    }
}

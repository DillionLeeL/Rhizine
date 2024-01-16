using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace Rhizine.WPF.ViewModels;

public class ShellDialogViewModel : ObservableObject
{
    private ICommand _closeCommand;

    public ICommand CloseCommand => _closeCommand ??= new RelayCommand(OnClose);

    public Action<bool?> SetResult { get; set; }

    public ShellDialogViewModel()
    {
    }

    private void OnClose()
    {
        bool result = true;
        SetResult(result);
    }
}
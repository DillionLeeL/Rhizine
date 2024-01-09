using CommunityToolkit.WinUI.UI.Controls;

using Microsoft.UI.Xaml.Controls;

using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Views;

public sealed partial class ListDetailsPage : Page
{
    public ListDetailsViewModel ViewModel { get; }

    public ListDetailsPage()
    {
        ViewModel = App.GetService<ListDetailsViewModel>() ?? throw new ArgumentNullException(nameof(ViewModel));
        InitializeComponent();
    }

    private void OnViewStateChanged(object sender, ListDetailsViewState e)
    {
        if (e == ListDetailsViewState.Both)
        {
            ViewModel.EnsureItemSelected();
        }
    }
}
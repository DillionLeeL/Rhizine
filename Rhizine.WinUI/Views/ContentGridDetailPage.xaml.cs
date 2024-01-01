using CommunityToolkit.WinUI.UI.Animations;

using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Rhizine.Core.Services.Interfaces;
//using Rhizine.WinUI.Services.Interfaces;
using Rhizine.WinUI.ViewModels;

namespace Rhizine.WinUI.Views;

public sealed partial class ContentGridDetailPage : Page
{
    public ContentGridDetailViewModel ViewModel
    {
        get;
    }

    public ContentGridDetailPage()
    {
        ViewModel = App.GetService<ContentGridDetailViewModel>() ?? throw new ArgumentNullException(nameof(ViewModel));
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        this.RegisterElementForConnectedAnimation("animationKeyContentGrid", itemHero);
    }

    protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
    {
        base.OnNavigatingFrom(e);
        if (e.NavigationMode == NavigationMode.Back)
        {
            //var navigationService = App.GetService<INavigationService>();

            if (ViewModel.Item != null)
            {
                ViewModel.NavigationService.NavigationSource.SetListDataItemForNextConnectedAnimation(ViewModel.Item);
            }
        }
    }
}
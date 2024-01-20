using Rhizine.Core.Services.Interfaces;

namespace Rhizine.MAUI.Views
{
    public partial class ListDetailsPage : ContentPage
    {
        ListDetailsViewModel ViewModel;
        public ListDetailsPage(ListDetailsViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Title = "ListDetails";
            BindingContext = ViewModel = viewModel;
        }
        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            await ViewModel.LoadDataAsync();
        }

    }

}

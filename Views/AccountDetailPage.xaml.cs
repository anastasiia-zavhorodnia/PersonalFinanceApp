using PersonalFinanceApp.ViewModels;

namespace PersonalFinanceApp.Views;

public partial class AccountDetailPage : ContentPage
{
    private readonly AccountDetailViewModel _viewModel; // Зберігаємо посилання на ViewModel

    public AccountDetailPage(AccountDetailViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing(); // Передаємо подію у ViewModel
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing(); // Передаємо подію у ViewModel
    }
}

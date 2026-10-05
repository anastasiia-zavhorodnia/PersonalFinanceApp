using PersonalFinanceApp.ViewModels;

namespace PersonalFinanceApp.Views;

public partial class AccountDetailPage : ContentPage
{
    public AccountDetailPage(AccountDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
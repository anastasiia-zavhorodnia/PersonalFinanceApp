using PersonalFinanceApp.ViewModels;

namespace PersonalFinanceApp.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel; // Зберігаємо посилання на ViewModel

    public MainPage(MainViewModel viewModel)
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

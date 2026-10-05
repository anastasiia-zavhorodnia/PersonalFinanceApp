namespace PersonalFinanceApp.Helpers;

public interface ILifecycleAware
{
    void OnAppearing();
    void OnDisappearing();
}
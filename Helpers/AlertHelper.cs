namespace PersonalFinanceApp.Helpers;

public static class AlertHelper
{
    public static Task ShowErrorAsync(string message) =>
        MainThread.InvokeOnMainThreadAsync(async () =>
        {
            if (Shell.Current is not null)
                await Shell.Current.DisplayAlert("Помилка", message, "OK");
        });
}
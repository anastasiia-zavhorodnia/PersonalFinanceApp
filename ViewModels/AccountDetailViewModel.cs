using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;
using System.Globalization;
using PersonalFinanceApp.Helpers; // <-- ДОДАНО ДЛЯ ДОСТУПУ ДО ІНТЕРФЕЙСУ
using System.Diagnostics; // <-- ДОДАНО ДЛЯ ДІАГНОСТИЧНОГО ЛОГУВАННЯ

namespace PersonalFinanceApp.ViewModels;

// ОНОВЛЕНО: додано реалізацію інтерфейсу ILifecycleAware
public partial class AccountDetailViewModel : ObservableObject, IQueryAttributable, ILifecycleAware
{
    private readonly AccountService _accountService;
    private int _id;

    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string type = string.Empty;
    [ObservableProperty] private string balance = string.Empty;

    public AccountDetailViewModel(AccountService accountService)
    {
        _accountService = accountService;
    }

    // Викликається після отримання Id (у кроці 4 її викличе IQueryAttributable)
    public void Load(int id)
    {
        var account = _accountService.GetById(id);
        if (account is null)
            return;

        _id = account.Id;
        Name = account.Name;
        Type = account.Type;
        Balance = account.Balance.ToString("0.##", CultureInfo.InvariantCulture);
    }

    // Метод, який автоматично викликається Shell при переході на сторінку
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) &&
            int.TryParse(value?.ToString(), out var id))
        {
            Load(id); // Ініціалізуємо поля екрана знайденим рахунком
        }
    }

    // <-- ДОДАНО: Метод викликається автоматично перед появою екрана деталей
    public void OnAppearing()
    {
        Debug.WriteLine("[AccountDetailViewModel] OnAppearing");
    }

    // <-- ДОДАНО: Метод викликається автоматично перед закриттям/приховуванням екрана деталей
    public void OnDisappearing()
    {
        Debug.WriteLine("[AccountDetailViewModel] OnDisappearing");
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(Name))
            return;

        var text = Balance.Trim().Replace(',', '.');
        if (!decimal.TryParse(text, NumberStyles.Number,
                              CultureInfo.InvariantCulture, out var parsed))
            return;

        var original = _accountService.GetById(_id);
        if (original is null)
            return;

        _accountService.Update(new Account
        {
            Id = _id,
            Name = Name.Trim(),
            Type = Type.Trim(),
            Balance = parsed,
            Transactions = original.Transactions
        });

        await Shell.Current.GoToAsync("..");   // повернення на попередній екран
    }
}

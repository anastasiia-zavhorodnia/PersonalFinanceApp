using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Helpers;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp.ViewModels;

public partial class AccountDetailViewModel
    : ObservableObject, IQueryAttributable, ILifecycleAware
{
    private readonly DatabaseService _db;
    private Account? _account;

    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string type = string.Empty;
    [ObservableProperty] private string balance = string.Empty;

    public AccountDetailViewModel(DatabaseService db)
    {
        _db = db;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) &&
            int.TryParse(value?.ToString(), out var id))
        {
            _ = LoadAsync(id);
        }
    }

    private async Task LoadAsync(int id)
    {
        var account = await _db.GetByIdAsync(id);
        if (account is null)
            return;

        _account = account;
        Name = account.Name;
        Type = account.Type;
        Balance = account.Balance.ToString("0.##", CultureInfo.InvariantCulture);
    }

    [RelayCommand]
    private async Task SaveAsync()
    {
        if (_account is null)
            return;

        if (string.IsNullOrWhiteSpace(Name))
        {
            await Shell.Current.DisplayAlert("Помилка",
                "Назва рахунку не може бути порожньою.", "OK");
            return;
        }

        decimal parsed = 0;
        if (!string.IsNullOrWhiteSpace(Balance))
        {
            var text = Balance.Trim().Replace(',', '.');
            if (!decimal.TryParse(text, NumberStyles.Number,
                                  CultureInfo.InvariantCulture, out parsed))
            {
                await Shell.Current.DisplayAlert("Помилка",
                    "Введіть коректну суму.", "OK");
                return;
            }
        }

        _account.Name = Name.Trim();
        _account.Type = Type.Trim();
        _account.Balance = parsed;

        await _db.UpdateAsync(_account);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task DeleteAsync()
    {
        if (_account is null)
            return;

        bool confirm = await Shell.Current.DisplayAlert("Видалення",
            $"Видалити рахунок «{_account.Name}»?", "Видалити", "Скасувати");
        if (!confirm)
            return;

        await _db.DeleteAsync(_account);
        await Shell.Current.GoToAsync("..");
    }

    public void OnAppearing()
    {
        Debug.WriteLine("[AccountDetailViewModel] OnAppearing");
    }

    public void OnDisappearing()
    {
        Debug.WriteLine("[AccountDetailViewModel] OnDisappearing");
    }
}
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PersonalFinanceApp.Helpers;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp.ViewModels;

public partial class AccountDetailViewModel
    : ObservableObject, IQueryAttributable, ILifecycleAware
{
    private readonly DatabaseService _db;
    private readonly ILogger<AccountDetailViewModel> _logger;
    private Account? _account;

    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string type = string.Empty;
    [ObservableProperty] private string balance = string.Empty;

    public AccountDetailViewModel(DatabaseService db, ILogger<AccountDetailViewModel> logger)
    {
        _db = db;
        _logger = logger;
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("id", out var value) &&
            int.TryParse(value?.ToString(), out var id))
        {
            _ = LoadAsync(id);
        }
        else
        {
            _logger.LogWarning("Екран деталей відкрито без коректного id");
        }
    }

    private async Task LoadAsync(int id)
    {
        var account = await _db.GetByIdAsync(id);
        if (account is null)
        {
            await AlertHelper.ShowErrorAsync("Не вдалося завантажити рахунок.");
            return;
        }

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
            await AlertHelper.ShowErrorAsync("Назва рахунку не може бути порожньою.");
            return;
        }

        decimal parsed = 0;
        if (!string.IsNullOrWhiteSpace(Balance))
        {
            var text = Balance.Trim().Replace(',', '.');
            if (!decimal.TryParse(text, NumberStyles.Number,
                                  CultureInfo.InvariantCulture, out parsed))
            {
                _logger.LogWarning("Користувач ввів некоректну суму: {Value}", Balance);
                await AlertHelper.ShowErrorAsync("Введіть коректну суму.");
                return;
            }
        }

        _account.Name = Name.Trim();
        _account.Type = Type.Trim();
        _account.Balance = parsed;

        if (!await _db.UpdateAsync(_account))
        {
            await AlertHelper.ShowErrorAsync("Не вдалося зберегти дані, спробуйте ще раз.");
            return;
        }

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

        if (!await _db.DeleteAsync(_account))
        {
            await AlertHelper.ShowErrorAsync("Не вдалося видалити рахунок, спробуйте ще раз.");
            return;
        }

        await Shell.Current.GoToAsync("..");
    }

    public void OnAppearing() => _logger.LogDebug("Екран деталей показано");

    public void OnDisappearing() => _logger.LogDebug("Екран деталей зник");
}
using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using PersonalFinanceApp.Helpers;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;
using System.Diagnostics;

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, ILifecycleAware
    {
        private const string DraftNameKey = "draft_account_name";
        private const string DraftBalanceKey = "draft_account_balance";

        private readonly DatabaseService _db;
        private readonly ILogger<MainViewModel> _logger;

        public ObservableCollection<Account> Accounts { get; } = new();

        [ObservableProperty]
        private string newAccountName = string.Empty;

        [ObservableProperty]
        private string newAccountBalance = string.Empty;

        public MainViewModel(DatabaseService db, ILogger<MainViewModel> logger)
        {
            _db = db;
            _logger = logger;
            LoadState();
        }

        private async Task LoadAccountsAsync()
        {
            var items = await _db.GetAllAsync();
            if (items is null)
            {
                // Технічні подробиці вже в лозі, користувачу показуємо коротке повідомлення
                await AlertHelper.ShowErrorAsync("Не вдалося завантажити рахунки. Спробуйте ще раз.");
                return;
            }

            Accounts.Clear();
            foreach (var item in items)
                Accounts.Add(item);
        }

        [RelayCommand]
        private async Task AddAccountAsync()
        {
            


            if (string.IsNullOrWhiteSpace(NewAccountName))
            {
                await AlertHelper.ShowErrorAsync("Введіть назву рахунку.");
                return;
            }

            decimal balance = 0;
            if (!string.IsNullOrWhiteSpace(NewAccountBalance))
            {
                var text = NewAccountBalance.Trim().Replace(',', '.');
                if (!decimal.TryParse(text, NumberStyles.Number,
                                      CultureInfo.InvariantCulture, out balance))
                {
                    _logger.LogWarning("Користувач ввів некоректну суму: {Value}", NewAccountBalance);
                    await AlertHelper.ShowErrorAsync("Введіть коректну суму.");
                    return;
                }
            }

            var account = new Account
            {
                Name = NewAccountName.Trim(),
                Type = "Банківський рахунок",
                Balance = balance
            };

            if (!await _db.InsertAsync(account))
            {
                await AlertHelper.ShowErrorAsync("Не вдалося зберегти дані, спробуйте ще раз.");
                return;   // поля не очищаємо, щоб користувач не втратив введене
            }

            Accounts.Add(account);
            NewAccountName = string.Empty;
            NewAccountBalance = string.Empty;
        }

        [RelayCommand]
        private async Task OpenAccountAsync(Account? account)
        {
            if (account is null)
                return;

            await Shell.Current.GoToAsync($"{Routes.AccountDetail}?id={account.Id}");
        }

        public void SaveState()
        {
            Preferences.Default.Set(DraftNameKey, NewAccountName);
            Preferences.Default.Set(DraftBalanceKey, NewAccountBalance);
            _logger.LogDebug("Чернетку форми збережено");
        }

        private void LoadState()
        {
            NewAccountName = Preferences.Default.Get(DraftNameKey, string.Empty);
            NewAccountBalance = Preferences.Default.Get(DraftBalanceKey, string.Empty);
            _logger.LogDebug("Чернетку форми відновлено");
        }

        public void OnAppearing()
        {
            _logger.LogInformation("Головний екран показано");
            _ = LoadAccountsAsync();
        }

        public void OnDisappearing()
        {
            _logger.LogDebug("Головний екран зник");
            SaveState();
        }
    }
}
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Helpers;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, ILifecycleAware
    {
        private const string DraftNameKey = "draft_account_name";
        private const string DraftBalanceKey = "draft_account_balance";

        private readonly DatabaseService _db;

        public ObservableCollection<Account> Accounts { get; } = new();

        [ObservableProperty]
        private string newAccountName = string.Empty;

        [ObservableProperty]
        private string newAccountBalance = string.Empty;

        public MainViewModel(DatabaseService db)
        {
            _db = db;
            LoadState();
        }

        private async Task LoadAccountsAsync()
        {
            try
            {
                var items = await _db.GetAllAsync();
                Accounts.Clear();
                foreach (var item in items)
                    Accounts.Add(item);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[MainViewModel] Помилка завантаження: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddAccountAsync()
        {
            if (string.IsNullOrWhiteSpace(NewAccountName))
                return;

            decimal balance = 0;
            if (!string.IsNullOrWhiteSpace(NewAccountBalance))
            {
                var text = NewAccountBalance.Trim().Replace(',', '.');
                if (!decimal.TryParse(text, NumberStyles.Number,
                                      CultureInfo.InvariantCulture, out balance))
                    return;
            }

            var account = new Account
            {
                Name = NewAccountName.Trim(),
                Type = "Банківський рахунок",
                Balance = balance
            };

            await _db.InsertAsync(account);   // після вставки account.Id заповнений базою
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
            Debug.WriteLine("[MainViewModel] Стан збережено");
        }

        private void LoadState()
        {
            NewAccountName = Preferences.Default.Get(DraftNameKey, string.Empty);
            NewAccountBalance = Preferences.Default.Get(DraftBalanceKey, string.Empty);
            Debug.WriteLine("[MainViewModel] Стан відновлено");
        }

        public void OnAppearing()
        {
            Debug.WriteLine("[MainViewModel] OnAppearing");
            _ = LoadAccountsAsync();   // перечитуємо базу: після редагування чи видалення список оновиться
        }

        public void OnDisappearing()
        {
            Debug.WriteLine("[MainViewModel] OnDisappearing");
            SaveState();
        }
    }
}
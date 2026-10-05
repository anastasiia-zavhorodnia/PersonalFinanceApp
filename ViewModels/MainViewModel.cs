using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;
using PersonalFinanceApp.Helpers;
using System.Diagnostics;

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject, ILifecycleAware
    {
        private readonly AccountService _accountService;

        // Ключі для легковагого збереження стану в Preferences API
        private const string DraftNameKey = "draft_account_name";
        private const string DraftBalanceKey = "draft_account_balance";

        public ObservableCollection<Account> Accounts => _accountService.Accounts;

        [ObservableProperty]
        private string newAccountName = string.Empty;

        [ObservableProperty]
        private string newAccountBalance = string.Empty;

        // ОНОВЛЕНО: Конструктор викликає відновлення стану форми при старті
        public MainViewModel(AccountService accountService)
        {
            _accountService = accountService;
            LoadState(); // Відновлення чернетки при ініціалізації
        }

        [RelayCommand]
        private void AddAccount()
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

            _accountService.Add(NewAccountName.Trim(), "Банківський рахунок", balance);

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

        // МЕТОДИ ДЛЯ РОБОТИ З PREFERENCES API (ЗБЕРЕЖЕННЯ / ВІДНОВЛЕННЯ)
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
        }

        // ОНОВЛЕНО: При згортанні/переході з головного екрана автоматично зберігаємо чернетку
        public void OnDisappearing()
        {
            Debug.WriteLine("[MainViewModel] OnDisappearing");
            SaveState();
        }
    }
}

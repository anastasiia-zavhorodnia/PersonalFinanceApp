using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Models;
using PersonalFinanceApp.Services;
using PersonalFinanceApp.Helpers; 

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly AccountService _accountService;

        public ObservableCollection<Account> Accounts => _accountService.Accounts;

        [ObservableProperty]
        private string newAccountName = string.Empty;

        [ObservableProperty]
        private string newAccountBalance = string.Empty;

        public MainViewModel(AccountService accountService)
        {
            _accountService = accountService;
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
    }
}

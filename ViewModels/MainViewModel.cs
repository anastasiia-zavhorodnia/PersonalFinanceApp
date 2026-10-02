using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PersonalFinanceApp.Models;

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Account> Accounts { get; } = new();

        [ObservableProperty]
        private string newAccountName = string.Empty;

        [ObservableProperty]
        private string newAccountBalance = string.Empty;

        [RelayCommand]
        private void AddAccount()
        {
            if (string.IsNullOrWhiteSpace(NewAccountName))
                return;

            // Порожнє поле балансу вважаємо нулем; кома й крапка приймаються обидві
            decimal balance = 0;
            if (!string.IsNullOrWhiteSpace(NewAccountBalance))
            {
                var text = NewAccountBalance.Trim().Replace(',', '.');
                if (!decimal.TryParse(text, NumberStyles.Number,
                                      CultureInfo.InvariantCulture, out balance))
                    return;
            }

            Accounts.Add(new Account
            {
                Name = NewAccountName.Trim(),
                Type = "Банківський рахунок",
                Balance = balance
            });

            NewAccountName = string.Empty;
            NewAccountBalance = string.Empty;
        }
    }
}
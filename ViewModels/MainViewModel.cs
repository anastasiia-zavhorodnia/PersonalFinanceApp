using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Models;

namespace PersonalFinanceApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<Account> Accounts { get; set; }

        public string NewAccountName { get; set; } = "";

        public decimal NewAccountBalance { get; set; }

        public MainViewModel()
        {
            Accounts = new ObservableCollection<Account>
            {
                new Account
                {
                    Name = "Готівка",
                    Type = "Готівка",
                    Balance = 1000
                },

                new Account
                {
                    Name = "Monobank",
                    Type = "Банківська картка",
                    Balance = 5000
                },

                new Account
                {
                    Name = "ПриватБанк",
                    Type = "Банківська картка",
                    Balance = 3000
                }
            };
        }

        [RelayCommand]
        public void AddAccount()
        {
            if (string.IsNullOrWhiteSpace(NewAccountName))
                return;

            Accounts.Add(new Account
            {
                Name = NewAccountName,
                Type = "Банківський рахунок",
                Balance = NewAccountBalance
            });

            NewAccountName = "";
            NewAccountBalance = 0;
        }
    }
}
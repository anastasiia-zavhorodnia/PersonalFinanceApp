using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using PersonalFinanceApp.Models;

namespace PersonalFinanceApp.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public ObservableCollection<Account> Accounts { get; set; }

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
    }
}
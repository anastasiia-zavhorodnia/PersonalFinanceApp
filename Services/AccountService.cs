using System.Collections.ObjectModel;
using PersonalFinanceApp.Models;

namespace PersonalFinanceApp.Services;

public class AccountService
{
    private int _nextId = 1;

    public ObservableCollection<Account> Accounts { get; } = new();

    public Account Add(string name, string type, decimal balance)
    {
        var account = new Account
        {
            Id = _nextId++,
            Name = name,
            Type = type,
            Balance = balance
        };
        Accounts.Add(account);
        return account;
    }

    public Account? GetById(int id) =>
        Accounts.FirstOrDefault(a => a.Id == id);

    // Заміна об'єкта в колекції, щоб CollectionView оновився
    public void Update(Account updated)
    {
        for (int i = 0; i < Accounts.Count; i++)
        {
            if (Accounts[i].Id == updated.Id)
            {
                Accounts[i] = updated;
                return;
            }
        }
    }
}
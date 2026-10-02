using System.Collections.Generic;

namespace PersonalFinanceApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public bool IsIncomeCategory { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new HashSet<Transaction>();

        public ICollection<BudgetCategory> BudgetCategories { get; set; } = new HashSet<BudgetCategory>();
    }
}
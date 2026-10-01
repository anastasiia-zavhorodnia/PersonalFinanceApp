using System.Collections.Generic;

namespace PersonalFinanceApp.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsIncomeCategory { get; set; }

        public ICollection<Transaction> Transactions { get; set; }

        public ICollection<BudgetCategory> BudgetCategories { get; set; }
    }
}
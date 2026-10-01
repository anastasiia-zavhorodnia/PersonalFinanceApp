using System;
using System.Collections.Generic;

namespace PersonalFinanceApp.Models
{
    public class Budget
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalAmount { get; set; }

        public ICollection<BudgetCategory> BudgetCategories { get; set; } = new List<BudgetCategory>();
    }
}


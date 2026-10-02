// Models/BudgetCategory.cs
namespace PersonalFinanceApp.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }

        public decimal LimitAmount { get; set; }

        public int BudgetId { get; set; }

        public Budget? Budget { get; set; }

        public int CategoryId { get; set; }

        public Category? Category { get; set; }
    }
}
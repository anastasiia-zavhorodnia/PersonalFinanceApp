using System.Collections.Generic;

namespace PersonalFinanceApp.Models
{
    public class FinancialGoal
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal TargetAmount { get; set; }

        public decimal CurrentAmount { get; set; }

        public DateTime TargetDate { get; set; }

        public string Description { get; set; }

        public ICollection<GoalContribution> Contributions { get; set; }
    }
}
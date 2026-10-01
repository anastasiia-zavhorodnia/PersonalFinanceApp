namespace PersonalFinanceApp.Models
{
    public class GoalContribution
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public int FinancialGoalId { get; set; }

        public FinancialGoal FinancialGoal { get; set; }
    }
}
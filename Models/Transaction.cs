namespace PersonalFinanceApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateTime Date { get; set; }

        public string Description { get; set; } = string.Empty;

        public bool IsIncome { get; set; }

        public int CategoryId { get; set; }

        public required Category Category { get; set; }

        public int AccountId { get; set; }

        public required Account Account { get; set; }

        public Transaction(Category category, Account account)
        {
            Category = category;
            Account = account;
        }
    }
}
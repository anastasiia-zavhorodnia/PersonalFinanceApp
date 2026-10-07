using SQLite;

namespace PersonalFinanceApp.Models
{
    [Table("Accounts")]
    public class Account
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Type { get; set; } = string.Empty;

        public decimal Balance { get; set; }

        [Ignore]
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
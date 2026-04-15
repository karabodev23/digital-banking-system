namespace Banking.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; } // Deposit, Withdraw, Transfer

        public DateTime CreatedAt { get; set; }
    }
}
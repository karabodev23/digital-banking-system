namespace Banking.Application.DTOs
{
    public class TransactionDto
    {
        public int Id { get; set; }

        public int FromAccountId { get; set; }

        public int ToAccountId { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
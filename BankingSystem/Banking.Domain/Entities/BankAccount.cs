namespace Banking.Domain.Entities
{
    public class BankAccount
    {
        public int Id { get; set; }

        public string AccountNumber { get; set; }

        public string AccountHolderName { get; set; }

        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
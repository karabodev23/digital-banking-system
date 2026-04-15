using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Domain.Entities;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Banking.Infrastructure.Services
{
    public class AccountService : IAccountService
    {
        private readonly BankingDbContext _context;

        public AccountService(BankingDbContext context)
        {
            _context = context;
        }

        public async Task<BankAccount> CreateAccountAsync(CreateAccountDto dto)
        {
            var account = new BankAccount
            {
                AccountHolderName = dto.AccountHolderName,
                AccountNumber = Guid.NewGuid().ToString().Substring(0, 10),
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            _context.BankAccounts.Add(account);
            await _context.SaveChangesAsync();

            return account;
        }

        public async Task<bool> DepositAsync(DepositDto dto)
        {
            var account = await _context.BankAccounts.FindAsync(dto.AccountId);

            if (account == null)
                return false;

            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            account.Balance += dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                FromAccountId = 0,
                ToAccountId = account.Id,
                Amount = dto.Amount,
                TransactionType = "Deposit",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> WithdrawAsync(WithdrawDto dto)
        {
            var account = await _context.BankAccounts.FindAsync(dto.AccountId);

            if (account == null)
                return false;

            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            if (account.Balance < dto.Amount)
                throw new Exception("Insufficient funds");

            account.Balance -= dto.Amount;

            _context.Transactions.Add(new Transaction
            {
                FromAccountId = account.Id,
                ToAccountId = 0,
                Amount = dto.Amount,
                TransactionType = "Withdraw",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return true;
        }

       
        public async Task<bool> TransferAsync(TransferDto dto)
        {
            var fromAccount = await _context.BankAccounts.FindAsync(dto.FromAccountId);
            var toAccount = await _context.BankAccounts.FindAsync(dto.ToAccountId);

            if (fromAccount == null || toAccount == null)
                return false;

            if (dto.Amount <= 0)
                throw new Exception("Amount must be greater than zero");

            if (fromAccount.Balance < dto.Amount)
                throw new Exception("Insufficient funds");

            
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Deduct
                fromAccount.Balance -= dto.Amount;

                // Add
                toAccount.Balance += dto.Amount;

                // Record transaction
                _context.Transactions.Add(new Transaction
                {
                    FromAccountId = fromAccount.Id,
                    ToAccountId = toAccount.Id,
                    Amount = dto.Amount,
                    TransactionType = "Transfer",
                    CreatedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<List<TransactionDto>> GetTransactionsAsync(int accountId)
        {
            var transactions = await _context.Transactions
                .Where(t => t.FromAccountId == accountId || t.ToAccountId == accountId)
                .OrderByDescending(t => t.CreatedAt)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    FromAccountId = t.FromAccountId,
                    ToAccountId = t.ToAccountId,
                    Amount = t.Amount,
                    TransactionType = t.TransactionType,
                    CreatedAt = t.CreatedAt
                })
                .ToListAsync();

            return transactions;
        }
    }
}
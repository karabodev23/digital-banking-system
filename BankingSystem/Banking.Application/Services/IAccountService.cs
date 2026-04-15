using Banking.Application.DTOs;
using Banking.Domain.Entities;

namespace Banking.Application.Services
{
    public interface IAccountService
    {
        Task<BankAccount> CreateAccountAsync(CreateAccountDto dto);

        Task<bool> DepositAsync(DepositDto dto);

        Task<bool> WithdrawAsync(WithdrawDto dto);

        Task<bool> TransferAsync(TransferDto dto);

        Task<List<TransactionDto>> GetTransactionsAsync(int accountId);
    }
}
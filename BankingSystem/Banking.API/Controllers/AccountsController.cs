using Microsoft.AspNetCore.Mvc;
using Banking.Application.DTOs;
using Banking.Application.Services;
using Banking.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Banking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly BankingDbContext _context;

   
        public AccountsController(
            IAccountService accountService,
            BankingDbContext context)
        {
            _accountService = accountService;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountDto dto)
        {
            var result = await _accountService.CreateAccountAsync(dto);
            return Ok(result);
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(DepositDto dto)
        {
            var result = await _accountService.DepositAsync(dto);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(WithdrawDto dto)
        {
            var result = await _accountService.WithdrawAsync(dto);
            return Ok(result);
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer(TransferDto dto)
        {
            var result = await _accountService.TransferAsync(dto);
            return Ok(result);
        }

        [HttpGet("{accountId}/transactions")]
        public async Task<IActionResult> GetTransactions(int accountId)
        {
            var result = await _accountService.GetTransactionsAsync(accountId);
            return Ok(result);
        }

       
        [HttpGet("{id}")]
        public async Task<IActionResult> GetAccount(int id)
        {
            var account = await _context.BankAccounts.FindAsync(id);

            if (account == null)
                return NotFound();

            return Ok(account);
        }
    }
}
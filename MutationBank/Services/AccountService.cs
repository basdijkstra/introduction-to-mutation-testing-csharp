using Microsoft.AspNetCore.Http.HttpResults;
using MutationBank.Exceptions;
using MutationBank.Models;
using MutationBank.Repositories;

namespace MutationBank.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ILogger<AccountService> _logger;

        public AccountService(IAccountRepository accountRepository, ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
        }

        public async Task<Account> GetAccountByIdAsync(string id)
        {
            if (!Guid.TryParse(id, out Guid result))
            {
                throw new BadRequestException($"'{id}' is not a valid account ID");
            }

            var accountPersisted = await this._accountRepository.GetAccountByIdAsync(id);

            if (accountPersisted == null)
            {
                throw new NotFoundException($"Account with ID '{id}' does not exist");
            }

            return accountPersisted;
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return await this._accountRepository.GetAllAccountsAsync();
        }

        public async Task<Account> AddAccountAsync(Account account)
        {
            return await this._accountRepository.AddAccountAsync(account);
        }

        public async Task DeleteAccountAsync(string id)
        {
            await this._accountRepository.DeleteAccountAsync(id);
        }

        public async Task<Account> DepositIntoAccountAsync(string id, decimal amount)
        {
            if (amount <= 0)
            {
                throw new BadRequestException("Amount should be greater than 0");
            }

            var accountPersisted = await GetAccountByIdAsync(id);

            accountPersisted.Balance += amount;

            return await AddAccountAsync(accountPersisted);
        }

        public async Task<Account> WithdrawFromAccountAsync(string id, decimal amount)
        {
            if (amount <= 0)
            {
                throw new BadRequestException("Amount should be greater than 0");
            }

            var accountPersisted = await GetAccountByIdAsync(id);

            if (accountPersisted.Type.Equals(AccountType.SAVINGS) && amount > accountPersisted.Balance)
            {
                throw new BadRequestException("You cannot overdraw from a savings account");
            }

            accountPersisted.Balance -= amount;

            return await AddAccountAsync(accountPersisted);
        }
    }
}

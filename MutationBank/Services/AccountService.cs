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
            return await this._accountRepository.GetAccountByIdAsync(id);
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
    }
}

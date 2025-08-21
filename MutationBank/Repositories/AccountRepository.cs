using MutationBank.Models;
using System.Collections.Concurrent;

namespace MutationBank.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ConcurrentDictionary<string, Account> accounts = new();

        public Task<Account> AddAccountAsync(Account account)
        {
            this.accounts[account.Id] = account;
            return Task.FromResult(account);
        }

        public Task DeleteAccountAsync(string id)
        {
            this.accounts[id] = null!;
            return Task.CompletedTask;
        }

        public Task<Account> GetAccountByIdAsync(string id)
        {
            Account account = this.accounts[id];
            return Task.FromResult(account);
        }

        public Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return Task.FromResult<IEnumerable<Account>>(this.accounts.Values.ToList());
        }

        public Task UpdateAccountAsync(Account account)
        {
            this.accounts[account.Id] = account;
            return Task.CompletedTask;
        }
    }
}

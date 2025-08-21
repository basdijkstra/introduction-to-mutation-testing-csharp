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

        public Task<Account?> GetAccountByIdAsync(string id)
        {
            var result = this.accounts.TryGetValue(id, out Account? account) ? account : null;
            return Task.FromResult(result);
        }

        public Task<IEnumerable<Account>> GetAllAccountsAsync()
        {
            return Task.FromResult<IEnumerable<Account>>(this.accounts.Values.ToList());
        }
    }
}

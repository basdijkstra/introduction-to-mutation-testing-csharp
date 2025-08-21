using MutationBank.Models;

namespace MutationBank.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync();

        Task<Account> GetAccountByIdAsync(string id);

        Task DeleteAccountAsync(string id);

        Task<Account> AddAccountAsync(Account account);

        Task UpdateAccountAsync(Account account);
    }
}

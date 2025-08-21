using MutationBank.Models;

namespace MutationBank.Services
{
    public interface IAccountService
    {
        Task<Account> GetAccountByIdAsync(string accountId);

        Task<IEnumerable<Account>> GetAllAccountsAsync();

        Task DeleteAccountAsync(string id);

        Task<Account> AddAccountAsync(Account account);
    }
}

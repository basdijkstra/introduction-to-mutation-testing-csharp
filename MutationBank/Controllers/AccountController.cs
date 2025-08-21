using Microsoft.AspNetCore.Mvc;
using MutationBank.Models;
using MutationBank.Services;

namespace MutationBank.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly ILogger<AccountController> _logger;

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            this.accountService = accountService;
            _logger = logger;
        }

        [HttpGet(Name = "GET all accounts")]
        [ProducesResponseType(typeof(IEnumerable<Account>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("GET all accounts");

            IEnumerable<Account> accounts = await this.accountService.GetAllAccountsAsync();

            if (accounts.Count() == 0)
            {
                return NoContent();
            }

            return Ok(accounts);
        }

        [HttpGet("{id}", Name = "GET account by ID")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation("GET account with ID {}", id);
            Account account = await this.accountService.GetAccountByIdAsync(id);
            return Ok(account);
        }

        [HttpPost(Name = "POST a new account")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            _logger.LogInformation("POST account with ID {}", account.Id);

            await this.accountService.AddAccountAsync(account);

            return this.CreatedAtRoute("GET account by ID", new { account.Id }, account);
        }

        [HttpPatch("{id}/deposit/{amount}", Name = "Deposit an amount into an account")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> Deposit(string id, decimal amount)
        {
            _logger.LogInformation("Deposit {} into account with ID {}", amount, id);

            var account = await this.accountService.DepositIntoAccountAsync(id, amount);

            return Ok(account);
        }

        [HttpPatch("{id}/withdraw/{amount}", Name = "Withdraw an amount from an account")]
        [ProducesResponseType(typeof(Account), StatusCodes.Status200OK)]
        public async Task<IActionResult> Withdraw(string id, decimal amount)
        {
            _logger.LogInformation("Withdraw {} from account with ID {}", amount, id);

            var account = await this.accountService.WithdrawFromAccountAsync(id, amount);

            return Ok(account);
        }

        [HttpDelete("{id}", Name = "DELETE an account by ID")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!Guid.TryParse(id, out Guid result))
            {
                return BadRequest();
            }

            await this.accountService.DeleteAccountAsync(id);
            return NoContent();
        }
    }
}

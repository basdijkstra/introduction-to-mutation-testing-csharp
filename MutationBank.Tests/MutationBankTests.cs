using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc.Testing;
using MutationBank.Models;
using MutationBank.Tests.Clients;
using Newtonsoft.Json;
using System.Net;
using static RestAssured.Dsl;

namespace MutationBank.Tests
{
    [TestFixture]
    public class MutationBankTests
    {
        private AccountClient accountClient;

        [SetUp]
        public void CreateClient()
        {
            this.accountClient = new AccountClient("http://localhost", 5131);
        }

        [Test]
        public void CreateNewCheckingAccount_WhenRetrieved_ShouldHaveZeroBalance()
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = this.accountClient.CreateAccount(account);

            var response = this.accountClient.GetAccount(id);

            response.StatusCode(HttpStatusCode.OK);
            response.Body("$.balance", NHamcrest.Is.EqualTo(0));
        }

        [Test]
        public void GetNonexistentAccount_shouldReturn404()
        {
            var id = Guid.NewGuid().ToString();

            var response = this.accountClient.GetAccount(id);

            response.StatusCode(HttpStatusCode.NotFound);
        }

        [Test]
        public void CreateNewAccount_RetrieveAllAccounts_ShouldContainCreatedAccount()
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = this.accountClient.CreateAccount(account);

            var response = this.accountClient.GetAllAccounts();

            response.StatusCode(HttpStatusCode.OK);
            response.Body("$[*].id", NHamcrest.Has.Item(NHamcrest.Is.EqualTo(account.Id)));
        }

        [Test]
        public void DeleteNonexistentAccount_ShouldReturn204()
        {
            var id = Guid.NewGuid().ToString();

            var response = this.accountClient.DeleteAccount(id);

            response.StatusCode(HttpStatusCode.NoContent);
        }

        [Test]
        public void DepositIntoCheckingAccount_WhenRetrieved_ShouldShowUpdatedBalance()
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = this.accountClient.CreateAccount(account);

            var response = this.accountClient.DepositToAccount(id, 10);

            response.StatusCode(HttpStatusCode.OK);
            response.Body("$.balance", NHamcrest.Is.EqualTo(10));
        }

        [Test]
        public void OverdrawOnSavingsAccount_ShouldReturn400_ShouldNotImpactBalance()
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.SAVINGS,
            };

            var id = this.accountClient.CreateAccount(account);

            this.accountClient.DepositToAccount(id, 10);

            var response = this.accountClient.WithdrawFromAccount(id, 20);

            response.StatusCode(HttpStatusCode.BadRequest);

            var getResponse = this.accountClient.GetAccount(id);

            getResponse.Body("$.balance", NHamcrest.Is.EqualTo(10));
        }

        [TestCase(100, 101, TestName = "A balance of 100 should be 101 after adding interest")]
        [TestCase(3000, 3060, TestName = "A balance of 3000 should be 3060 after adding interest")]
        [TestCase(6000, 6180, TestName = "A balance of 6000 should be 6180 after adding interest")]
        public void AddInterestToSavingsAccount_ShouldUpdateBalanceCorrectly(int amountToDeposit, int expectedBalance)
        {
            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.SAVINGS,
            };

            var id = this.accountClient.CreateAccount(account);

            this.accountClient.DepositToAccount(id, amountToDeposit);

            var response = this.accountClient.AddInterestToAccount(id);

            response.Body("$.balance", NHamcrest.Is.EqualTo(expectedBalance));
        }
    }
}

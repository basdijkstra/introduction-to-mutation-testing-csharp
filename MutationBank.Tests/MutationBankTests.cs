using Microsoft.AspNetCore.Mvc.Testing;
using MutationBank.Models;
using static RestAssured.Dsl;

namespace MutationBank.Tests
{
    [TestFixture]
    public class MutationBankTests
    {
        [Test]
        public void RetrieveListOfAccounts()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            Given(httpClient)
                .When()
                .Get("http://localhost:5131/account")
                .Then()
                .StatusCode(204);
        }

        [Test]
        public void CreateThenRetrieveAccount()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = Given(httpClient)
                .Body(account)
                .When()
                .Post("http://localhost:5131/account")
                .Then()
                .StatusCode(201)
                .Extract().Body("$.id");

            Given(httpClient)
                .When()
                .Get($"http://localhost:5131/account/{id}")
                .Then()
                .Log(RestAssured.Response.Logging.ResponseLogLevel.All)
                .StatusCode(200);
        }

        [Test]
        public void GetNonexistentAccount()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var accountId = Guid.NewGuid().ToString();

            var id = Given(httpClient)
                .When()
                .Get($"http://localhost:5131/account/{accountId}")
                .Then()
                .Log(RestAssured.Response.Logging.ResponseLogLevel.All)
                .StatusCode(404);
        }

        [Test]
        public void DepositTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = Given(httpClient)
                .Body(account)
                .When()
                .Post("http://localhost:5131/account")
                .Then()
                .StatusCode(201)
                .Extract().Body("$.id");

            Given(httpClient)
                .When()
                .Patch($"http://localhost:5131/account/{id}/deposit/50")
                .Then()
                .Log(RestAssured.Response.Logging.ResponseLogLevel.All)
                .StatusCode(200)
                .Body("$.balance", NHamcrest.Is.EqualTo(50));
        }

        [Test]
        public void WithdrawTest()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            var account = new Account
            {
                Id = Guid.NewGuid().ToString(),
                Type = AccountType.CHECKING,
            };

            var id = Given(httpClient)
                .Body(account)
                .When()
                .Post("http://localhost:5131/account")
                .Then()
                .StatusCode(201)
                .Extract().Body("$.id");

            Given(httpClient)
                .When()
                .Patch($"http://localhost:5131/account/{id}/deposit/50")
                .Then()
                .StatusCode(200)
                .Body("$.balance", NHamcrest.Is.EqualTo(50));

            Given(httpClient)
                .When()
                .Patch($"http://localhost:5131/account/{id}/withdraw/30")
                .Then()
                .Log(RestAssured.Response.Logging.ResponseLogLevel.All)
                .StatusCode(200)
                .Body("$.balance", NHamcrest.Is.EqualTo(20));
        }
    }
}

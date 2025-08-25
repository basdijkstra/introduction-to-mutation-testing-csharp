using Microsoft.AspNetCore.Mvc.Testing;
using MutationBank.Models;
using RestAssured.Response;
using static RestAssured.Dsl;

namespace MutationBank.Tests.Clients
{
    public class AccountClient : AccountBase
    {
        private readonly HttpClient httpClient;

        public AccountClient(string baseUri, int port) : base(baseUri, port)
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            this.httpClient = webAppFactory.CreateDefaultClient();
        }

        public string CreateAccount(Account account)
        {
            return (string)Given(this.httpClient)
                .Spec(RequestSpec())
                .Body(account)
                .When()
                .Post("/account")
                .Then()
                .StatusCode(201)
                .Extract().Body("$.id");
        }

        public VerifiableResponse GetAccount(string accountId)
        {
            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Get($"/account/{accountId}");
        }

        public VerifiableResponse GetAllAccounts()
        {
            Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Get("/account")
                .Then().Log(RestAssured.Response.Logging.ResponseLogLevel.All);

            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Get("/account");
        }

        public VerifiableResponse AddInterestToAccount(string accountId)
        {
            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Patch($"/account/{accountId}/interest");
        }

        public VerifiableResponse DeleteAccount(string accountId)
        {
            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Delete($"/account/{accountId}");
        }

        public VerifiableResponse DepositToAccount(string accountId, int amount)
        {
            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Patch($"/account/{accountId}/deposit/{amount}");
        }

        public VerifiableResponse WithdrawFromAccount(string accountId, int amount)
        {
            return Given(this.httpClient)
                .Spec(RequestSpec())
                .When()
                .Patch($"/account/{accountId}/withdraw/{amount}");
        }
    }
}

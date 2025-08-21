using Microsoft.AspNetCore.Mvc.Testing;
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
    }
}

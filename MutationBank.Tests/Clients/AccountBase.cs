using RestAssured.Request.Builders;

namespace MutationBank.Tests.Clients
{
    public abstract class AccountBase
    {
        private readonly string baseUri;
        private readonly int port;

        protected AccountBase(string baseUri, int port)
        {
            this.baseUri = baseUri;
            this.port = port;
        }

        public RequestSpecification RequestSpec()
        {
            return new RequestSpecBuilder()
                .WithBaseUri(this.baseUri)
                .WithPort(this.port)
                .WithContentType("application/json")
                .Build();
        }
    }
}

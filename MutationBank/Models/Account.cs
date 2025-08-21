namespace MutationBank.Models
{
    public class Account
    {
        public string Id { get; set; } = string.Empty;
        public AccountType Type { get; set; }
        public decimal Balance { get; set; }
    }
}

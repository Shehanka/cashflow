namespace CashFlow.Models
{
    public class User
    {
        public Guid Id { get; init; }
        public string Username { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Password { get; init; }
        public string Email { get; init; }
        public DateTimeOffset CreatedDate { get; init; } 
    }
}
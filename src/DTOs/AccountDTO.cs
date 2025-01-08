namespace WebApiApp.DTOs
{
    public class CreateAccountDTO
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Name { get; set; }
    }
}

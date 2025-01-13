using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebApiApp.DTOs
{
    public class CreateAccountDTO
    {
        public required string Email { get; set; }
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public required string Password { get; set; }
        public string? Name { get; set; }
    }

    public class UpdateAccountDTO
    {
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }
        public string? Name { get; set; }
        public bool? IsActive { get; set; }
    }

    public class AccountDTO
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonIgnore]
        public string? HashedPassword { get; set; }
    }
}

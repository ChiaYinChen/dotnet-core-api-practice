using System.ComponentModel.DataAnnotations;

namespace WebApiApp.Models
{
    public class LoginRequest
    {
        [Required]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }

    public class AuthCallbackRequest
    {
        [Required]
        public string code { get; set; } = string.Empty;
        public string? state { get; set; }
    }
}

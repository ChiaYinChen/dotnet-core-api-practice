using System.ComponentModel.DataAnnotations;

namespace WebApiApp.Models
{
    public class Token
    {   
        [Required]
        public required string access_token { get; set; }
        [Required]
        public required string refresh_token { get; set; }
        [Required]
        public required string token_type { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiApp.Entities
{
    [Table("ACCOUNT")]
    public class Account : BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        [StringLength(255)]
        public required string Email { get; set; }

        [StringLength(255)]
        [Column("Password")]
        public string? HashedPassword { get; set; }

        [StringLength(32)]
        public string? Name { get; set; } = null;

        [Required]
        public bool IsActive { get; set; } = true;
    }
}

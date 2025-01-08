using System;
using System.ComponentModel.DataAnnotations;

namespace WebApiApp.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        [Required]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}

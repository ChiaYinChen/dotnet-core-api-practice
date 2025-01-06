using System;
using System.ComponentModel.DataAnnotations;
using WebApiApp.Helpers;

namespace WebApiApp.Models
{
    public abstract class BaseEntity
    {
        [Required]
        public DateTime CreatedAt { get; set; } = TimeHelper.Now();

        [Required]
        public DateTime UpdatedAt { get; set; } = TimeHelper.Now();
    }
}

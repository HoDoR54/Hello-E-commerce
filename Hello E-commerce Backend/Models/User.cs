using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_Admin_Dashboard.Models
{
    public enum UserRole
    {
        Admin,
        Customer
    }

    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        public UserRole Role { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; } = false;
        public bool IsWarned { get; set; } = false;
        public int? WarningLevel { get; set; }

        public bool IsBanned { get; set; } = false;
        public int? BannedDays { get; set; }

        public Admin AdminProfile { get; set; }
        public Customer CustomerProfile { get; set; }
    }
}

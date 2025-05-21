using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_Admin_Dashboard.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSuperAdmin { get; set; }

        [ForeignKey("CreatedBy")]
        public Guid? CreatedBy { get; set; }

        public Admin? CreatedByAdmin { get; set; }
        public ICollection<AdminAction> AdminActions { get; set; }

        public ICollection<Admin> AdminsCreated { get; set; }
    }
}

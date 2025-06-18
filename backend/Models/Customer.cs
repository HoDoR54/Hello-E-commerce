using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E_commerce_Admin_Dashboard.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Phone]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Range(0, int.MaxValue)]
        public int LoyaltyPoints { get; set; }

        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Purchase> Purchases { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<CustomerAddressDetail> CustomerAddresseDetails { get; set; }
        public ICollection<CustomerAction> CustomerActions { get; set; }
        public ICollection<AdminAction> AdminActions { get; set; }
    }
}

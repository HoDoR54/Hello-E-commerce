using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace E_commerce_Admin_Dashboard.Models
{
    public enum AdminActionType
    {
        Warn,
        Ban
    }

    public abstract class AdminAction
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Admin")]
        public Guid AdminId { get; set; }
        public Admin Admin { get; set; }

        [ForeignKey("TargetUser")]
        public Guid TargetCustomerId { get; set; }
        public User TargetCustomer { get; set; }
        public AdminActionType ActionType { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class Warn : AdminAction
    {
        public string? Reason { get; set; }
        public int Severity { get; set; } // 1-3
    }

    public class Ban : AdminAction
    {
        public string Reason { get; set; }

        public int DurationDays { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class PasswordResetToken
    {
        [Key]
        public long PasswordResetId { get; set; }

        [Required]
        public string MemberID { get; set; } = null!;

        [Required]
        [StringLength(200)]
        public string Token { get; set; } = null!;

        public DateTime ExpiryDate { get; set; }
        public bool Used { get; set; } = false;

        public Member? Member { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class NotificationEvent
    {
        [Key]
        [Display(Name = "事件編號")]
        public long EventID { get; set; }

        [Display(Name = "通知類型")]
        [Required]
        public NotificationType NotificationType { get; set; }

        [Required]
        [StringLength(100)]
        public string Title { get; set; } = null!;

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "通知內容")]
        [Required]
        public string Content { get; set; } = null!;

        [StringLength(255)]
        public string? LinkUrl { get; set; }

        [Required]
        public bool IsRead { get; set; } = false;

        public DateTime? ReadAt { get; set; }

        [StringLength(30)]
        public string? RelatedEntityType { get; set; }

        [StringLength(36)]
        public string? RelatedEntityId { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
    }
}

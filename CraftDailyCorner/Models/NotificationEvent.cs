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
        public NotificationType NotificationType { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "通知內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
    }
}

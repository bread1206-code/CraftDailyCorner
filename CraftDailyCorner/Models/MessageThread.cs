using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class MessageThread
    {
        [Key]
        [Display(Name = "連線編號")]
        public int ThreadID { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; } = null!;

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string? ProductID { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "最後訊息時間")]
        public DateTime LastMessageAt { get; set; }
        
        [Display(Name = "最後訊息預覽")]
        [StringLength(50, ErrorMessage = "最後訊息預覽不可超過 50 字")]
        public string? LastMessagePreview { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual List<Message> Messages { get; set; } = new List<Message>();
        public virtual Product? Product { get; set; }
        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
    }
}

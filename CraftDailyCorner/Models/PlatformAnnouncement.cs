using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class PlatformAnnouncement
    {
        [Key]
        [Display(Name = "公告編號")]
        public int AnnouncementID { get; set; }
        [StringLength(50)]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;
        [Column(TypeName = "nvarchar(max)")]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "狀態")]
        public PlatformAnnouncementStatus Status { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "建立者")]
        public string CreatedBy { get; set; }= null!;

        public virtual Member Member { get; set; } = null!;
    }
}

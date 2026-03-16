using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class PlatformAnnouncementStatus
    {
        [Key]
        [Display(Name = "狀態代碼")]
        public byte StatusID { get; set; }
        [Display(Name = "狀態碼")]
        [StringLength(50)]
        public string StatusCode { get; set; } = null!;
        [Display(Name = "狀態名稱")]
        [StringLength(50)]
        public string StatusName { get; set; } = null!;
        [Display(Name = "描述")]
        [StringLength(200)]
        public string? Description { get; set; }
        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }
        public virtual List<PlatformAnnouncement> PlatformAnnouncements { get; set; } = new List<PlatformAnnouncement>();
    }
}

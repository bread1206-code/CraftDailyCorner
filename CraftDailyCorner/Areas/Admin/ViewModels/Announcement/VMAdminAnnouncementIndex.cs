using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Announcement
{
    public class VMAdminAnnouncementIndex
    {
        public List<VMAdminAnnouncementIndexItem> Items { get; set; } = new();
    }

    public class VMAdminAnnouncementIndexItem
    {
        [Display(Name = "公告編號")]
        public int AnnouncementID { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Display(Name = "受眾")]
        public byte AudienceType { get; set; }

        public string AudienceName { get; set; } = null!;

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        public string StatusName { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "建立者")]
        public string CreatedBy { get; set; } = null!;

        public string? CreatedByName { get; set; }

        [Display(Name = "發布時間")]
        public DateTime? PublishedAt { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime? UpdatedAt { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Announcement
{
    public class VMAdminAnnouncementUpsert
    {
        public int? AnnouncementID { get; set; }

        [Display(Name = "標題")]
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50, ErrorMessage = "標題不可超過 50 字")]
        public string Title { get; set; } = null!;

        [Display(Name = "內容")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "受眾")]
        [Required(ErrorMessage = "請選擇受眾")]
        public byte AudienceType { get; set; }

        [Display(Name = "狀態")]
        [Required(ErrorMessage = "請選擇狀態")]
        public byte StatusID { get; set; }

        public SelectList? AudienceOptions { get; set; }
        public SelectList? StatusOptions { get; set; }
    }
}
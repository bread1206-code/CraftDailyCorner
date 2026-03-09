using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.Areas.Admin.ViewModels.HomepageBanner
{
    public class VMAdminHomepageBannerUpsert
    {
        public int? BannerID { get; set; }

        [Display(Name = "主標題")]
        [Required(ErrorMessage = "請輸入主標題")]
        [StringLength(50, ErrorMessage = "主標題不可超過 50 字")]
        public string Title { get; set; } = null!;

        [Display(Name = "副標題")]
        [StringLength(50, ErrorMessage = "副標題不可超過 50 字")]
        public string? Subtitle { get; set; }

        [Display(Name = "圖片")]
        public IFormFile? ImageFile { get; set; }

        // Edit 頁面顯示目前圖片用
        public string? CurrentImageUrl { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; } = 2;
    }
}
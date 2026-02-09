using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.ViewModels.Front.CreatorPost
{
    public class VMCreatorPostEdit
    {
        public string PostID { get; set; } = null!;

        [Required]
        [StringLength(50)]
        [Display(Name = "日誌標題")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "日誌內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "目前封面")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "更換封面圖片")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "誰可以看")]
        public CreatorPostVisibility Visibility { get; set; }
    }
}
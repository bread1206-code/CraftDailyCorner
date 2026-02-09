using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.ViewModels.Front.CreatorPost
{
    public class VMCreatorPostCreate
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "日誌標題")]
        public string Title { get; set; } = null!;

        [Required]
        [Display(Name = "日誌內容")]
        public string Content { get; set; } = null!;

        [Required]
        [Display(Name = "封面圖片")]
        public IFormFile ImageFile { get; set; } = null!;

        [Display(Name = "誰可以看")]
        public CreatorPostVisibility Visibility { get; set; } = CreatorPostVisibility.Public;
    }
}
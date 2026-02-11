using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMCreatorPostEdit
    {
        [Required]
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "內容")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "目前封面")]
        public string CurrentImageUrl { get; set; } = null!;

        [Display(Name = "更換封面圖片")]
        public IFormFile? NewImageFile { get; set; }

        [Display(Name = "觀看權限")]
        [Required]
        public CreatorPostVisibility Visibility { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedAt { get; set; }

        // UI 用
        public string CoverImagePath =>
            $"/Photos/05CreatorPost/Medium/{CurrentImageUrl}.png";
    }
}

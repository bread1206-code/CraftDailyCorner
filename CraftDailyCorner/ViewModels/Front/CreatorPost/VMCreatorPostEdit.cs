using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.ViewModels.Front.CreatorPost
{
    public class VMCreatorPostEdit
    {
        [Required]
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50, ErrorMessage = "標題不可超過 50 字")]
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

        // UI 專用屬性

        public string CoverImagePath =>
            $"/Photos/Post/Medium/{CurrentImageUrl}.png";

        public string VisibilityText =>
            Visibility switch
            {
                CreatorPostVisibility.Public => "公開",
                CreatorPostVisibility.Followers => "僅追蹤者",
                CreatorPostVisibility.Private => "私人",
                _ => ""
            };
    }
}
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMPostDetail
    {
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Display(Name = "內容")]
        public string Content { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        [Display(Name = "創作者")]
        public string CreatorName { get; set; } = null!;

        [Display(Name = "發佈時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "最後更新")]
        public DateTime UpdatedAt { get; set; }

        // UI 輔助屬性

        public string CoverImagePath =>
            $"/Photos/05CreatorPost/Large/{ImageUrl}.png";

        public string CreatorProfileUrl =>
            $"/Creator/Public/{CreatorName}";

        public bool IsUpdated =>
            UpdatedAt > CreatedAt;
    }
}

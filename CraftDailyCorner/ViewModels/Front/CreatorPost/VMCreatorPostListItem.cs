using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front.CreatorPost
{
    public class VMCreatorPostListItem
    {
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Display(Name = "封面圖片")]
        public string ImageUrl { get; set; } = null!;

        [Display(Name = "觀看權限")]
        public CreatorPostVisibility Visibility { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "留言數")]
        public int CommentCount { get; set; }

        // UI 用（非資料庫欄位）
        public string VisibilityText =>
            Visibility switch
            {
                CreatorPostVisibility.Public => "公開",
                CreatorPostVisibility.Followers => "僅追蹤者",
                CreatorPostVisibility.Private => "私人",
                _ => ""
            };

        public string CoverImagePath =>
            $"/Photos/Post/Medium/{ImageUrl}.png";
    }
}
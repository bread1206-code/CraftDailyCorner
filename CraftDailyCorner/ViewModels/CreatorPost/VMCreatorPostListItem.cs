using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMCreatorPostListItem
    {
        public string PostID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        [Display(Name = "觀看權限")]
        public CreatorPostVisibility Visibility { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "留言數")]
        public int CommentCount { get; set; }

        // UI 屬性
        public string CoverImagePath =>
            $"/Photos/Post/Medium/{ImageUrl}.png";

        public string VisibilityText =>
            Visibility switch
            {
                CreatorPostVisibility.Public => "公開",
                CreatorPostVisibility.Followers => "僅追蹤者",
                CreatorPostVisibility.Private => "私人",
                _ => ""
            };

        public string VisibilityBadgeClass =>
            Visibility switch
            {
                CreatorPostVisibility.Public => "bg-success",
                CreatorPostVisibility.Followers => "bg-warning",
                CreatorPostVisibility.Private => "bg-secondary",
                _ => "bg-dark"
            };
    }
}

using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioListItem
    {
        public string PortfolioID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "作品數量")]
        public int ItemCount { get; set; }
        [Display(Name = "封面圖片URL")]
        public string? CoverImageUrl { get; set; }

        public string VisibilityText =>
            Visibility switch
            {
                CreatorPostVisibility.Public => "公開",
                CreatorPostVisibility.Followers => "僅追蹤者",
                CreatorPostVisibility.Private => "私人",
                _ => ""
            };

        public CreatorPostVisibility Visibility { get; set; }
    }
}

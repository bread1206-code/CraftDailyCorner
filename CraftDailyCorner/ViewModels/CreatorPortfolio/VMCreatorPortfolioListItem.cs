using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioListItem
    {
        public string PortfolioID { get; set; } = null!;

        public string CreatorID { get; set; } = null!;

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

        public string CoverImagePath =>
            string.IsNullOrEmpty(CoverImageUrl)
                ? "/images/default-cover.jpg"
                : $"/Photos/06Portfolio/{CreatorID}/Medium/{CoverImageUrl}.webp";

        public string VisibilityText =>
            Visibility switch
            {
                CreatorVisibility.Public => "公開",
                CreatorVisibility.Followers => "僅追蹤者",
                CreatorVisibility.Private => "私人",
                _ => ""
            };

        public CreatorVisibility Visibility { get; set; }
    }
}
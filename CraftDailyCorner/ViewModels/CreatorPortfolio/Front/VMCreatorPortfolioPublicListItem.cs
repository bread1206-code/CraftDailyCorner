using CraftDailyCorner.ViewModels.Reaction;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMCreatorPortfolioPublicListItem
    {
        public string PortfolioID { get; set; } = null!;

        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Display(Name = "創作者")]
        public string CreatorName { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public int ItemCount { get; set; }

        public string? CoverImageUrl { get; set; }
        //內文預覽（用 p-preview 多行截斷）
        public string? Preview { get; set; }

        //心情反應（標題旁 icon + total）
        public VMReactionButton ReactionSummary { get; set; } = new VMReactionButton();
        public string CoverImagePath =>
            string.IsNullOrEmpty(CoverImageUrl)
                ? "/images/default-cover.jpg"
                : $"/Photos/06Portfolio/Medium/{CoverImageUrl}.png";

        public string Url =>
            $"/Portfolio/Detail/{PortfolioID}";
    }
}

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

        public string CreatorID { get; set; } = null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public int ItemCount { get; set; }

        public string? CoverImageUrl { get; set; }

        public string? Preview { get; set; }

        public VMReactionButton ReactionSummary { get; set; } = new VMReactionButton();

        public string CoverImagePath =>
            string.IsNullOrEmpty(CoverImageUrl)
                ? "/images/default-cover.jpg"
                : $"/Photos/06Portfolio/{CreatorID}/Medium/{CoverImageUrl}.png";

        public string Url => $"/Portfolio/Detail/{PortfolioID}";
    }
}
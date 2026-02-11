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

        public string Url =>
            $"/Portfolio/Detail/{PortfolioID}";
    }
}

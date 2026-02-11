using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioCreate
    {
        [Display(Name = "標題")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "簡介")]
        [Required]
        public string Description { get; set; } = null!;

        [Display(Name = "觀看權限")]
        [Required]
        public CreatorPostVisibility Visibility { get; set; }
    }
}

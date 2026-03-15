using CraftDailyCorner.ImageManagementCore.ViewModels;
using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioEdit
    {
        [Required]
        public string PortfolioID { get; set; } = null!;

        [Display(Name = "標題")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "簡介")]
        [Required]
        public string? Description { get; set; }

        [Display(Name = "觀看權限")]
        [Required]
        public CreatorVisibility Visibility { get; set; }

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedAt { get; set; }
        public List<VMCreatorPortfolioItemEdit> Items { get; set; } = new();

        public VMImageManagement? ImageManagement { get; set; }
    }
}

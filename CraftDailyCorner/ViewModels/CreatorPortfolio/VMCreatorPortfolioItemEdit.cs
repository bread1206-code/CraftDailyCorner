using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioItemEdit
    {
        public string ItemID { get; set; } = null!;

        public string PortfolioID { get; set; } = null!;

        [Display(Name = "標題")]
        [Required]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        [Display(Name = "簡介")]
        public string? Description { get; set; }

        [Display(Name = "排序")]
        [Range(0, 255)]
        public byte SortOrder { get; set; }

        [Display(Name = "目前圖片")]
        public string CurrentImageUrl { get; set; } = null!;

        [Display(Name = "更換圖片")]
        public IFormFile? NewImageFile { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ImagePath =>
            $"/Photos/06Portfolio/Medium/{CurrentImageUrl}.png";
    }
}

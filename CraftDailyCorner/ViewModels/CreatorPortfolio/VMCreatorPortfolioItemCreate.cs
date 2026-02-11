using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioItemCreate
    {
        [Required]
        public string PortfolioID { get; set; } = null!;

        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50)]
        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        [Required(ErrorMessage = "請上傳圖片")]
        public IFormFile ImageFile { get; set; } = null!;

        [Range(0, 255)]
        public byte SortOrder { get; set; }
    }
}

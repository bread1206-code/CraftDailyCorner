using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Member
{
    public class VMProductReviewForm
    {
        [Required]
        public string OrderID { get; set; } = null!;

        [Required]
        public string ProductID { get; set; } = null!;

        public long? ReviewID { get; set; }

        [Required(ErrorMessage = "請選擇星等")]
        [Range(1, 5, ErrorMessage = "星等需介於 1 到 5 之間")]
        public byte Rating { get; set; }

        [StringLength(1000, ErrorMessage = "評論最多 1000 字")]
        public string? Comment { get; set; }
    }
}
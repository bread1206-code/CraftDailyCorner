using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class ProductReview
    {
        [Key]
        [Display(Name = "評價編號")]
        public long ReviewID { get; set; }

        [Range(1, 5, ErrorMessage = "星等需介於 1 到 5 之間")]
        [Display(Name = "星等")]
        public byte Rating { get; set; }

        [Display(Name = "評論")]
        [Column(TypeName = "nvarchar(max)")]
        public string? Comment { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime? UpdatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        [StringLength(12, MinimumLength = 12)]
        [Column(TypeName = "nchar(12)")]
        [Display(Name = "訂單編號")]
        public string OrderID { get; set; } = null!;

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual Order Order { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
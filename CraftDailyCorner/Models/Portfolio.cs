using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Portfolio
    {
        [Key]
        [StringLength(36, MinimumLength = 36)]
        [Column(TypeName = "nchar(36)")]

        [Display(Name = "作品集編號")]
        public string PortfolioID { get; set; } = null!;

        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "必填欄位")]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "簡介")]
        public string? Description { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; } = null!;

        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
        public virtual List<PortfolioItem>? PortfolioItem { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class HomepageBanner
    {
        [Key]
        [Display(Name = "橫幅編號")]
        public int BannerID { get; set; }

        [StringLength(40, MinimumLength = 40)]
        [Column(TypeName = "nchar(40)")]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "圖片URL")]
        public string ImageUrl { get; set; }= null!;

        [StringLength(50, MinimumLength = 1)]
        [Required(ErrorMessage = "必填欄位")]
        [Display(Name = "標題")]
        public string Title { get; set; }= null!;

        [StringLength(50, MinimumLength = 1)]
        [Display(Name = "副標題")]
        public string? Subtitle { get; set; }

        [Display(Name = "狀態")]
        public byte StatusID { get; set; }=0;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "建立者")]
        public string CreatedBy { get; set; }= null!;

        public virtual Member Member { get; set; } = null!;
        public virtual HomepageBannerStatus HomepageBannerStatus { get; set; } = null!;
    }
}

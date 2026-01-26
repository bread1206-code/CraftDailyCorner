using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class FavoriteProduct
    {
        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; }= null!;

        [StringLength(10, MinimumLength = 10)]
        [Column(TypeName = "nchar(10)")]
        [Display(Name = "商品編號")]
        public string ProductID { get; set; }= null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}

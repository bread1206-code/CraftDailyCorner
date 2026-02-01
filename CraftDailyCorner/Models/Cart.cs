using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Cart
    {
        [Key]
        [Display(Name = "購物車編號")]
        public int CartID { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; }= null!;

        public virtual Member Member { get; set; } = null!;
        public virtual List<CartItem>? CartItems { get; set; }
    }
}

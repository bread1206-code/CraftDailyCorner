using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class FollowCreator
    {
        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; }= null!;

        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; }= null!;

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
    }
}

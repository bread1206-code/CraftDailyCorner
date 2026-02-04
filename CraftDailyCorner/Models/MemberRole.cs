using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class MemberRole
    {
        [StringLength(8,MinimumLength =8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        [StringLength(2,MinimumLength =2)]
        [Column(TypeName = "nchar(2)")]
        [Display(Name = "角色編號")]
        public string RoleID { get; set; } = null!;

        [Display(Name = "指派時間")]
        public DateTime AssignedAt { get; set; }

        public virtual Member Member { get; set; } = null!;
        public virtual Role Role { get; set; } = null!;
    }
}

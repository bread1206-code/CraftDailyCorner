using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class MemberRoleHistory
    {
        [Key]
        [Display(Name = "會員角色紀錄編號")]
        public long MemberRoleHistoryID { get; set; }

        [Display(Name ="動作")]
        public MemberRoleHistoryAction Action { get; set; }

        [Display(Name ="操作時間")]
        public DateTime OperatedAt { get; set; }

        [StringLength(8,MinimumLength =8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "會員編號")]
        public string MemberID { get; set; } = null!;

        [Column(TypeName = "nchar(2)")]
        [StringLength(2, MinimumLength = 2)]
        [Display(Name = "角色編號")]
        public string RoleID { get; set; } = null!;

        [Display(Name ="操作人員身份")]
        public MemberRoleHistoryOperated OperatedBy { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "操作人員會員編號")]
        public string? OperatorMemberID { get; set; }

        // 導覽屬性
        public virtual Role Role { get; set; } = null!;
        public virtual Member? Member { get; set; }
    }
}

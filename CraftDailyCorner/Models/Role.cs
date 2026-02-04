using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class Role
    {
        [Key]
        [Column(TypeName = "nchar(2)")]
        [StringLength(2,MinimumLength =2)]
        [Display(Name = "角色編號")]
        [Required(ErrorMessage ="必填")]
        public string RoleID { get; set; } = null!;

        [StringLength(10)]
        [Display(Name = "角色名稱")]
        [Required(ErrorMessage = "必填")]
        public string RoleName { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "描述")]
        public string? Description { get; set; }

        public virtual List<MemberRole>? MemberRoles { get; set; }
        public virtual List<MemberRoleHistory>? MemberRoleHistories { get; set; }
    }
}

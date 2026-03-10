using System.ComponentModel.DataAnnotations;
namespace CraftDailyCorner.Models.enums
{
    public enum MemberRoleHistoryOperated : byte
    {
        [Display(Name = "系統")]
        System = 0,//系統
        [Display(Name = "管理者")]
        Admin = 1 //管理者
    }
}
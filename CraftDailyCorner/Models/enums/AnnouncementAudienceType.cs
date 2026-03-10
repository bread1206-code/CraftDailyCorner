using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models.enums
{
    public enum AnnouncementAudienceType : byte
    {
        [Display(Name = "所有會員")]
        AllMembers = 1,

        [Display(Name = "僅創作者")]
        CreatorsOnly = 2,

        [Display(Name = "僅管理者")]
        AdminsOnly = 3
    }
}
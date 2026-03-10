using System.ComponentModel.DataAnnotations;
namespace CraftDailyCorner.Models.enums
{
    public enum CreatorPostVisibility : byte
    {
        [Display(Name = "公開")]
        Public = 0, //公開

        [Display(Name = "追蹤者限定")]
        Followers = 1,  //追蹤者

        [Display(Name = "隱私/草稿")]
        Private = 2 //隱私
    }
}
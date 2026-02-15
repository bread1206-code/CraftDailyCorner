using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models.Enums
{
    public enum CommentReportReason
    {
        [Display(Name = "垃圾訊息")]
        Spam = 1,

        [Display(Name = "人身攻擊")]
        Abuse = 2,

        [Display(Name = "色情內容")]
        Adult = 3,

        [Display(Name = "其他")]
        Other = 4
    }
}
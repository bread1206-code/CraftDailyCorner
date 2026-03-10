using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models.enums
{
    public enum ReportTargetType: byte
    {
        [Display(Name = "留言")]
        Comment = 1,
        [Display(Name = "創作日誌")]
        Post = 2,
        [Display(Name = "商品")]
        Product = 3,
        [Display(Name = "作品集")]
        Portfolio = 4
    }
}

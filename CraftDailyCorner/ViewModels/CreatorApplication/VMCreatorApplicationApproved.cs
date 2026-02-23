using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorApplication
{
    // 創作者申請已通過
    public class VMCreatorApplicationApproved
    {
        [Display(Name = "品牌名稱")]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "核准時間")]
        public DateTime ReviewedAt { get; set; }
    }
}

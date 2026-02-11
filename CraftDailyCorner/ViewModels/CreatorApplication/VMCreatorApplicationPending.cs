using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorApplication
{
    // 創作者申請審核中
    public class VMCreatorApplicationPending
    {
        [Display(Name = "品牌名稱")]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "申請時間")]
        public DateTime AppliedAt { get; set; }
    }
}

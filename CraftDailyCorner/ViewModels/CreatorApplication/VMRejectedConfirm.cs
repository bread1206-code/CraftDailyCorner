using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorApplication
{
    public class VMRejectedConfirm
    {
        [Required]
        public int ApplicationID { get; set; }

        public string BrandName { get; set; } = string.Empty;

        // 顯示原因（CreatorApplication.ReviewNote）
        public string ReviewNote { get; set; } = string.Empty;
    }
}
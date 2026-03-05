using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Tag
{
    public class VMAdminTagEdit
    {
        public int TagID { get; set; }

        [Required(ErrorMessage = "請輸入標籤名稱")]
        [StringLength(20, ErrorMessage = "標籤名稱不可超過 20 字")]
        [Display(Name = "標籤名稱")]
        public string TagName { get; set; } = null!;
    }
}
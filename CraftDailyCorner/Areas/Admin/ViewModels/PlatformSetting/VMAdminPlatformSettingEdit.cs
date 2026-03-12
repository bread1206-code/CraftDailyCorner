using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.PlatformSetting
{
    public class VMAdminPlatformSettingEdit
    {
        public int SettingID { get; set; }

        [Display(Name = "設定名稱")]
        public string SettingKey { get; set; } = null!;

        [Display(Name = "設定值")]
        [Required(ErrorMessage = "請輸入設定值")]
        [StringLength(50, ErrorMessage = "設定值不可超過 50 字")]
        public string SettingValue { get; set; } = null!;

        [Display(Name = "資料型態")]
        public string DataType { get; set; } = null!;

        [Display(Name = "類別")]
        public byte CategoryID { get; set; }

        public string CategoryName { get; set; } = null!;

        [Display(Name = "敘述")]
        public string? Description { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [Display(Name = "更新者")]
        public string UpdatedBy { get; set; } = null!;

        public string? UpdatedByName { get; set; }

        public bool IsBoolType =>
            string.Equals(DataType, "bool", StringComparison.OrdinalIgnoreCase);

        public bool IsImageType =>
            string.Equals(DataType, "image", StringComparison.OrdinalIgnoreCase);

        [Display(Name = "上傳 LOGO")]
        public IFormFile? LogoFile { get; set; }

        public SelectList? BoolOptions { get; set; }

        public string? HintText { get; set; }

        public string? SuggestedRange { get; set; }
    }
}
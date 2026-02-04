using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class PlatformSettingCategory
    {

        [Key]
        [Display(Name = "類別代碼")]
        public byte CategoryID { get; set; }
        [Display(Name = "類別碼")]
        public string CategoryCode { get; set; } = null!;
        [Display(Name = "類別名稱")]
        public string CategoryName { get; set; } = null!;
        [Display(Name = "描述")]
        public string? Description { get; set; }
        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }

        public virtual List<PlatformSetting>? PlatformSettings { get; set; }
    }
}

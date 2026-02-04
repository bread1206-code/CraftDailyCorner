using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class PlatformSetting
    {
        [Key]
        [Display(Name = "設定編號")]
        public int SettingID { get; set; }

        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "設定名稱")]
        public string SettingKey { get; set; }= null!;

        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "設定值")]
        public string SettingValue { get; set; } = null!;

        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "資料型態")]
        public string DataType { get; set; } = null!;

        [Display(Name = "類別")]
        public byte CategoryID { get; set; }

        [Display(Name = "敘述")]
        public string? Description { get; set; }

        [Display(Name = "更新時間")]
        public DateTime UpdatedAt { get; set; }

        [StringLength(8, MinimumLength = 8)]
        [Column(TypeName = "nchar(8)")]
        [Display(Name = "更新者")]
        public string UpdatedBy { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;
        public virtual PlatformSettingCategory PlatformSettingCategory { get; set; } = null!;
    }
}

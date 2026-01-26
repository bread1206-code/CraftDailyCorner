using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.ViewModels
{
    public class VMCreateMember
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "暱稱最少1個字，最多20個字")]
        [Required]
        [Display(Name = "暱稱")]
        public string DisplayName { get; set; } = null!;

        [StringLength(40)]
        [EmailAddress(ErrorMessage = "電子郵件格式錯誤")]
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "必填欄位")]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "密碼為8-20碼")]
        public string PasswordHash { get; set; } = null!;

        [Column(TypeName = "nchar(40)")]
        [Display(Name = "頭像")]
        public string? ImageUrl { get; set; }

        [StringLength(10, MinimumLength = 10, ErrorMessage = "手機號碼為10碼阿拉伯數字")]
        [Display(Name = "手機號碼")]
        [RegularExpression("[0 - 9]{10}",ErrorMessage = "手機號碼格式錯誤")]
        public string? Phone { get; set; }

        [Display(Name = "生日")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "性別")]
        [Required(ErrorMessage = "請選擇性別")]
        public PrivacyGender Gender { get; set; }
        

    }
}

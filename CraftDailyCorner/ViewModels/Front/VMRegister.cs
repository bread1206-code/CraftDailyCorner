using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.ViewModels.Front
{
    //註冊會員用
    public class VMRegister
    {
        [StringLength(20, MinimumLength = 1, ErrorMessage = "暱稱最少1個字，最多20個字")]
        [Required(ErrorMessage = "必填")]
        [Display(Name = "暱稱")]
        public string DisplayName { get; set; } = null!;

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "電子郵件格式錯誤")]
        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "必填")]
        public string Email { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼為8-20碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "必填")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼為8-20碼")]
        [Compare("Password", ErrorMessage = "兩次密碼不一致")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = null!;

        [StringLength(10, MinimumLength = 10, ErrorMessage = "手機號碼為10碼阿拉伯數字")]
        [Display(Name = "手機號碼")]
        [Required(ErrorMessage = "必填")]
        [RegularExpression("[0-9]{10}", ErrorMessage = "手機號碼格式錯誤")]
        public string? Phone { get; set; }

        [Display(Name = "性別")]
        [Required(ErrorMessage = "請選擇性別")]
        public PrivacyGender? Gender { get; set; }


    }
}

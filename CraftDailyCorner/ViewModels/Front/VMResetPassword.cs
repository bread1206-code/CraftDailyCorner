using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    //重設密碼畫面用
    public class VMResetPassword
    {
        [Key]
        [Required]
        [HiddenInput]
        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "請輸入新密碼")]
        [Display(Name = "請輸入新密碼")]
        [StringLength(12, MinimumLength = 8, ErrorMessage = "密碼為8-12碼")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; } = null!;

        [Display(Name = "請再次輸入新密碼")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "密碼與確認密碼不符")]
        [Required(ErrorMessage = "請輸入確認密碼")]
        public string ConfirmPassword { get; set; } = null!;
    }
}

using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Member
{
    public class VMChangePassword
    {
        [Required(ErrorMessage = "請輸入目前密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "目前密碼")]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "請輸入新密碼")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "新密碼長度需介於 8~20 字元")]
        [DataType(DataType.Password)]
        [Display(Name = "新密碼")]
        public string NewPassword { get; set; } = string.Empty;

        [Required(ErrorMessage = "請再次輸入新密碼")]
        [DataType(DataType.Password)]
        [Display(Name = "確認新密碼")]
        [Compare(nameof(NewPassword), ErrorMessage = "兩次輸入的新密碼不一致")]
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
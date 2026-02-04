using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    //忘記密碼流程第一步（輸入 Email 或帳號請求重設）
    public class VMForgetPassword
    {
        [Required]
        [EmailAddress]
        [Display(Name = "請輸入您的Email")]
        public string Email { get; set; } = null!;
    }
}

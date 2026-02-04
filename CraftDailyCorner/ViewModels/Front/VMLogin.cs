using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    //登入畫面用
    public class VMLogin
    {

        [Display(Name = "帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(40, MinimumLength = 10, ErrorMessage = "帳號為10-40碼")]
        public string Account { get; set; } = null!;

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "密碼為3-20碼")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberAccount { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMForgetPassword
    {
        [Key]
        [Required]
        [EmailAddress]
        [Display(Name = "請輸入您的Email")]
        public string Email { get; set; } = null!;
    }
}

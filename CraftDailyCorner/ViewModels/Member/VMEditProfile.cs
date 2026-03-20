using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Member
{
    public class VMEditProfile
    {
        [Required]
        public string MemberID { get; set; } = null!;

        [Display(Name = "暱稱")]
        [Required(ErrorMessage = "請輸入您的暱稱")]
        [MaxLength(20)]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "電子郵件")]
        public string Email { get; set; } = null!;

        [Display(Name = "手機號碼")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "請輸入10碼手機號碼")]
        public string Phone { get; set; } = null!;

        // 目前頭像（顯示用）
        public string? ImageUrl { get; set; }

        // 上傳用（不存 DB）
        [Display(Name = "會員頭像")]
        public IFormFile? AvatarFile { get; set; }

    }
}

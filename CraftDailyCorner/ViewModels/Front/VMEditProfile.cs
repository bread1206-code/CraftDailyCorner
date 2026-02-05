using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMEditProfile
    {
        [Required]
        public string MemberID { get; set; } = null!;

        [Display(Name = "顯示名稱")]
        [Required(ErrorMessage = "請輸入顯示名稱")]
        [MaxLength(20)]
        public string DisplayName { get; set; } = null!;

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email 格式不正確")]
        public string Email { get; set; } = null!;

        [Display(Name = "聯絡電話")]
        [MaxLength(20)]
        public string? Phone { get; set; }

        // 目前頭像（顯示用）
        public string? ImageUrl { get; set; }

        // 上傳用（不存 DB）
        [Display(Name = "會員頭像")]
        public IFormFile? AvatarFile { get; set; }

    }
}

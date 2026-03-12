using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorBrandEdit
    {
        // 唯讀顯示用
        public string CreatorID { get; set; } = string.Empty;

        [Display(Name = "品牌名稱")]
        public string BrandName { get; set; } = string.Empty;

        // 現有圖片 key（DB 存的那個）
        public string? ImageUrl { get; set; }
        public DateTime UpdatedAt { get; set; }

        // 顯示圖片用（/Photos/...）
        public string CurrentImagePath =>
            string.IsNullOrWhiteSpace(ImageUrl)
                ? $"/Photos/03CreatorBrand/{CreatorID}/Large/default.png"
                : $"/Photos/03CreatorBrand/{CreatorID}/Large/{ImageUrl}.png?v={UpdatedAt.Ticks}";

        [Display(Name = "品牌圖片")]
        public IFormFile? BrandImage { get; set; }

        [Display(Name = "品牌簡介")]
        [StringLength(500, ErrorMessage = "簡介最多 500 字")]
        public string? BrandIntro { get; set; }

        [Display(Name = "銀行代碼")]
        [StringLength(10, ErrorMessage = "銀行代碼最多 10 碼")]
        public string? BankCode { get; set; }

        [Display(Name = "銀行帳號")]
        [StringLength(30, ErrorMessage = "銀行帳號最多 30 碼")]
        public string? BankAccount { get; set; }
    }
}
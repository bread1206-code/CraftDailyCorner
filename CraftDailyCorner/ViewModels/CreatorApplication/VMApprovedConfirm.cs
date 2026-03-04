using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace CraftDailyCorner.ViewModels.CreatorApplication
{
    public class VMApprovedConfirm
    {
        [Required]
        public int ApplicationID { get; set; }

        // ===== 唯讀顯示（從 CreatorApplication 帶入）=====
        public string BrandName { get; set; } = string.Empty;
        public string BrandIntro { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        // ===== 必填（3欄）=====
        [Required(ErrorMessage = "請上傳品牌圖片")]
        public IFormFile? BrandImageFile { get; set; }

        [Required(ErrorMessage = "請填寫銀行代碼")]
        [StringLength(10, ErrorMessage = "銀行代碼長度過長")]
        public string BankCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "請填寫銀行帳號")]
        [StringLength(30, ErrorMessage = "銀行帳號長度過長")]
        public string BankAccount { get; set; } = string.Empty;
    }
}
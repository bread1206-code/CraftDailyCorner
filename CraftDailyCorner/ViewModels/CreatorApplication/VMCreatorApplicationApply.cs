using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorApplication
{
    // 申請成為創作者（填寫表單）
    public class VMCreatorApplicationApply
    {
        [Required(ErrorMessage = "請輸入品牌名稱")]
        [StringLength(20, ErrorMessage = "品牌名稱不可超過 20 字")]
        [Display(Name = "品牌名稱")]
        public string BrandName { get; set; } = null!;

        [Required(ErrorMessage = "請輸入品牌簡介")]
        [StringLength(1000, ErrorMessage = "品牌簡介不可超過 1000 字")]
        [Display(Name = "品牌簡介")]
        public string BrandIntro { get; set; } = null!;

        [Required(ErrorMessage = "請上傳作品範例圖片")]
        [Display(Name = "作品範例圖片")]
        public IFormFile PortfolioSample { get; set; } = null!;

        [Required(ErrorMessage = "請選擇創作起始日")]
        [DataType(DataType.Date)]
        [Display(Name = "創作起始日")]
        public DateTime StartDate { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMCreatorProductForm : IValidatableObject
    {
        // 基本資料
        [Display(Name = "商品編號")]
        public string? ProductID { get; set; }

        [Required]
        [StringLength(40)]
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;

        [Required]
        [Display(Name = "商品描述")]
        public string Description { get; set; } = null!;

        [Required]
        [Range(0.01, 9999999)]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        [Display(Name = "價格")]
        public decimal Price { get; set; }

        // 狀態
        [Required]
        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        // 直接使用 SelectListItem
        public List<SelectListItem> StatusSelectList { get; set; } = new();

        // 庫存
        [Required]
        [Display(Name = "庫存數量")]
        public int StockQty { get; set; }

        [Required]
        [Display(Name = "警戒數量")]
        public int AlertQty { get; set; }

        // 建立用圖片
        [Display(Name = "商品圖片")]
        public List<IFormFile>? ImageFiles { get; set; }

        // Edit 用圖片清單
        [Display(Name = "現有商品圖片")]
        public List<VMProductImageItem> ExistingImages { get; set; } = new();

        // 分類
        [Display(Name = "商品分類")]
        public List<int> SelectedCategoryIds { get; set; } = new();
        public List<VMCategoryGroup> CategoryGroups { get; set; } = new();

        // 標籤
        [Display(Name = "商品標籤")]
        public List<int> SelectedTagIds { get; set; } = new();
        public List<SelectListItem> TagSelectList { get; set; } = new();

        // 驗證
        public IEnumerable<ValidationResult> Validate(ValidationContext context)
        {
            if (SelectedCategoryIds.Count > 3)
                yield return new ValidationResult(
                    "分類最多3個",
                    new[] { nameof(SelectedCategoryIds) });

            if (SelectedTagIds.Count > 10)
                yield return new ValidationResult(
                    "標籤最多10個",
                    new[] { nameof(SelectedTagIds) });

            if (AlertQty > StockQty)
                yield return new ValidationResult(
                    "警戒值不可大於庫存",
                    new[] { nameof(AlertQty) });
        }
    }
}
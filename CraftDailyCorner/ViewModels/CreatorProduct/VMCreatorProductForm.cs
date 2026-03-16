using CraftDailyCorner.ImageManagementCore.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorProduct
{
    public class VMCreatorProductForm : IValidatableObject
    {
        // 基本資料
        [Display(Name = "商品編號")]
        public string? ProductID { get; set; }

        [Required(ErrorMessage = "商品名稱為必填")]
        [StringLength(40)]
        [Display(Name = "商品名稱")]
        public string ProductName { get; set; } = null!;

        [Required(ErrorMessage = "商品描述為必填")]
        [Display(Name = "商品描述")]
        public string Description { get; set; } = null!;

        [Display(Name = "價格")]
        [Required(ErrorMessage = "請輸入商品價格")]
        [Range(1, 1000000,ErrorMessage = "價格最少為1，最多為1,000,000")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal Price { get; set; }

        [Display(Name = "商品成本")]
        [Required(ErrorMessage = "請輸入商品成本")]
        [Range(1,1000000, ErrorMessage = "商品成本必須大於 0")]
        [DisplayFormat(DataFormatString = "{0:0}", ApplyFormatInEditMode = true)]
        public decimal CostPrice { get; set; }

        // 狀態
        [Required]
        [Display(Name = "狀態")]
        public byte StatusID { get; set; }

        // 直接使用 SelectListItem
        public List<SelectListItem> StatusSelectList { get; set; } = new();

        // 庫存
        [Range(1, 10000, ErrorMessage = "庫存最少為1，最多為10,000")]
        [Display(Name = "庫存數量")]
        [Required(ErrorMessage = "庫存數量為必填")]
        public int StockQty { get; set; }

        [Display(Name = "警戒數量")]
        [Required(ErrorMessage = "警戒數量為必填")]
        public int AlertQty { get; set; }

        // 建立用圖片
        [Display(Name = "商品圖片")]
        public List<IFormFile>? ImageFiles { get; set; }

        // Edit 用圖片清單
        [Display(Name = "現有商品圖片")]
        [ValidateNever]
        //public List<VMProductImageItem> ExistingImages { get; set; } = new();
        public VMImageManagement? ImageManagement { get; set; }
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
            if (SelectedCategoryIds == null || SelectedCategoryIds.Count == 0)
                yield return new ValidationResult(
                    "請至少選擇一個分類",
                    new[] { nameof(SelectedCategoryIds) });

            if (SelectedCategoryIds!.Count > 3)
                yield return new ValidationResult(
                    "分類最多3個",
                    new[] { nameof(SelectedCategoryIds) });

            if (SelectedTagIds.Count > 10)
                yield return new ValidationResult(
                    "標籤最多10個",
                    new[] { nameof(SelectedTagIds) });

            if (string.IsNullOrEmpty(ProductID)) // 只有建立才驗證
            {
                if (ImageFiles == null || ImageFiles.Count == 0)
                    yield return new ValidationResult(
                        "請至少上傳 1 張商品圖片",
                        new[] { nameof(ImageFiles) });
            }

            if (AlertQty > StockQty)
                yield return new ValidationResult(
                    "警戒值不可大於庫存",
                    new[] { nameof(AlertQty) });
        }
    }
}
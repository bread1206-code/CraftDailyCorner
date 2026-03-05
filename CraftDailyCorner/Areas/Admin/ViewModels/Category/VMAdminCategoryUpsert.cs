using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Areas.Admin.ViewModels.Category
{
    public class VMAdminCategoryUpsert
    {
        public int? CategoryID { get; set; }

        [Display(Name = "分類名稱")]
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(20, ErrorMessage = "最多 20 個字")]
        public string CategoryName { get; set; } = null!;

        [Display(Name = "大分類")]
        public int? ParentCategoryID { get; set; } // null 代表大分類

        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; } = true;

        // 下拉選單：只放「大分類」+ null 選項
        public SelectList? ParentCategoryOptions { get; set; }
    }
}
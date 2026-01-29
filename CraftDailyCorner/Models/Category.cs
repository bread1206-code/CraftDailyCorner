using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class Category
    {
        [Key]
        [Display(Name = "分類編號")]
        public int CategoryID { get; set; }

        [Display(Name = "分類名稱")]
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(20)]
        public string CategoryName { get; set; }= null!;
        [Display(Name = "總分類編號")]
        public int? ParentCategoryID { get; set; }
        [Display(Name ="是否啟用")]
        public bool IsActive { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public virtual List<ProductCategory>? ProductCategory { get; set; }
        public Category? ParentCategory { get; set; }
        public List<Category>? SubCategories { get; set; }

    }
}

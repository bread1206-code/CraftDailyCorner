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

        public virtual List<ProductCategory>? ProductCategory { get; set; }
    }
}

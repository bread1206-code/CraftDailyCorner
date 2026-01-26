using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.Models
{
    public class Tag
    {
        [Key]
        [Display(Name = "標籤編號")]
        public int TagID { get; set; }

        [Display(Name = "標籤名稱")]
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(20)]
        public string TagName { get; set; } = null!;

        public virtual List<ProductTag>? ProductTag { get; set; }
    }
}

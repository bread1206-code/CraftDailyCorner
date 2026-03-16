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
        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }

        public virtual List<ProductTag> ProductTags { get; set; } = new List<ProductTag>();
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CraftDailyCorner.Models
{
    public class AutoReplyTemplate
    {
        [Key]
        [Display(Name = "範本編號")]
        public int TemplateID { get; set; }
        [Required(ErrorMessage = "必填欄位")]
        [StringLength(50)]
        [Display(Name = "標題")]
        public string Title { get; set; } = null!;
        [Required(ErrorMessage = "必填欄位")]
        [Column(TypeName = "nvarchar(max)")]
        [Display(Name = "內容")]
        public string Content { get; set; } = null!;
        [Display(Name = "是否啟用")]
        public bool IsActive { get; set; }
        [Display(Name = "觸發條件")]
        public AutoReplyTemplateTriggerType TriggerType { get; set; }
        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; }
        [StringLength(6, MinimumLength = 6)]
        [Column(TypeName = "nchar(6)")]
        [Display(Name = "創作者編號")]
        public string CreatorID { get; set; } = null!;


        public virtual CreatorProfile CreatorProfile { get; set; } = null!;
    }
}

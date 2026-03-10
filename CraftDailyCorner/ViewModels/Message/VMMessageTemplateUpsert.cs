using System.ComponentModel.DataAnnotations;
using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.ViewModels.Message
{
    public class VMMessageTemplateUpsert
    {
        public int? TemplateID { get; set; }

        [Display(Name = "模板標題")]
        [Required(ErrorMessage = "請輸入模板標題")]
        [StringLength(30, ErrorMessage = "模板標題不可超過 30 字")]
        public string Title { get; set; } = null!;

        [Display(Name = "模板內容")]
        [Required(ErrorMessage = "請輸入模板內容")]
        [StringLength(500, ErrorMessage = "模板內容不可超過 500 字")]
        public string Content { get; set; } = null!;

        [Display(Name = "模板類型")]
        [Required(ErrorMessage = "請選擇模板類型")]
        public AutoReplyTemplateTriggerType TriggerType { get; set; }

        [Display(Name = "啟用")]
        public bool IsActive { get; set; } = true;
    }
}
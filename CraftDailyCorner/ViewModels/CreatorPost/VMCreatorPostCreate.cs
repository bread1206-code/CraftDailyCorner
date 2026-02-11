using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMCreatorPostCreate
    {
        [Display(Name = "標題")]
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(50, ErrorMessage = "標題不可超過 50 字")]
        public string Title { get; set; } = null!;

        [Display(Name = "內容")]
        [Required(ErrorMessage = "請輸入內容")]
        public string Content { get; set; } = null!;

        [Display(Name = "封面圖片")]
        [Required(ErrorMessage = "請上傳封面圖片")]
        public IFormFile ImageFile { get; set; } = null!;

        [Display(Name = "觀看權限")]
        [Required]
        public CreatorPostVisibility Visibility { get; set; }
    }
}

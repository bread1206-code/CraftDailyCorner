using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.DTOs
{
    public class CreatePostCommentDTO
    {
        [Required]
        [StringLength(36, MinimumLength = 36)]
        public string PostID { get; set; } = null!;

        [Required(ErrorMessage = "請輸入留言內容")]
        [StringLength(500, ErrorMessage = "留言不可超過 500 字")]
        public string Content { get; set; } = null!;
    }
}
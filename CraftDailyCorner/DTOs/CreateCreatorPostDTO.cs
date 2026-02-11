using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.DTOs
{
    public class CreateCreatorPostDTO
    {
        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required] // 封面必填
        public string ImageUrl { get; set; } = null!;

        [Required]
        public CreatorPostVisibility Visibility { get; set; }
    }
}

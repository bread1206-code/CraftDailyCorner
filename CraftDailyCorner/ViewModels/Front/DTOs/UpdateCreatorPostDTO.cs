using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front.DTOs
{
    public class UpdateCreatorPostDTO
    {
        [Required]
        [StringLength(36)]
        public string PostID { get; set; } = null!;

        [Required]
        [StringLength(50, MinimumLength = 1)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [Required]
        public string ImageUrl { get; set; } = null!;

        [Required]
        public CreatorPostVisibility Visibility { get; set; }
    }
}

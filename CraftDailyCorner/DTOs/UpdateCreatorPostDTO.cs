using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class UpdateCreatorPostDTO
    {
        public string PostID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public CreatorPostVisibility Visibility { get; set; }

        public IFormFile? NewImageFile { get; set; }
    }
}

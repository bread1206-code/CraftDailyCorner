using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class CreateCreatorPostDTO
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public CreatorPostVisibility Visibility { get; set; }
        
        public IFormFile ImageFile { get; set; } = null!;
    }
}

using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class CreateCreatorPortfolioDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CreatorVisibility Visibility { get; set; }
    }
}

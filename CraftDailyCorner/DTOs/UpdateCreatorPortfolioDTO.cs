using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.DTOs
{
    public class UpdateCreatorPortfolioDTO
    {
        public string PortfolioID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CreatorPostVisibility Visibility { get; set; }
    }
}

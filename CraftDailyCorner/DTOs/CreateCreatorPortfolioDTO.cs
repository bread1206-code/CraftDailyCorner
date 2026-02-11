namespace CraftDailyCorner.DTOs
{
    public class CreateCreatorPortfolioDTO
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public CreatorPostVisibility Visibility { get; set; }
    }
}

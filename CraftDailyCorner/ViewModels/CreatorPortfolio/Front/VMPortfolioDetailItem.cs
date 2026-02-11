namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioDetailItem
    {
        public string ItemID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string ImageUrl { get; set; } = null!;

        public string ImagePath =>
            $"/Photos/06Portfolio/Medium/{ImageUrl}.png";
    }
}

namespace CraftDailyCorner.ViewModels.CreatorPortfolio
{
    public class VMCreatorPortfolioItemListItem
    {
        public string ItemID { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public byte SortOrder { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public string ImagePath =>
            $"/Photos/06Portfolio/Medium/{ImageUrl}.png";
    }
}

namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioDetailItem
    {
        public long ItemID { get; set; }
        public string ImageUrl { get; set; } = null!;
        public string CreatorID { get; set; } = null!;

        public string ImagePath =>
            $"/Photos/06Portfolio/{CreatorID}/Large/{ImageUrl}.png";
    }
}
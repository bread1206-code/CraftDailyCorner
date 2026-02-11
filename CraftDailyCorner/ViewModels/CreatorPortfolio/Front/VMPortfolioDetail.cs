namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioDetail
    {
        public string PortfolioID { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public List<VMPortfolioDetailItem> Items { get; set; } = new();
    }
}

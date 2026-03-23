namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioIndexQuery
    {
        public string? PortfolioKeyword { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 9;
    }
}

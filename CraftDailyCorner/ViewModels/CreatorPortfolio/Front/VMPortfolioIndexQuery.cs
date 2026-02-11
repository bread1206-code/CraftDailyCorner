namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioIndexQuery
    {
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 16;
    }
}

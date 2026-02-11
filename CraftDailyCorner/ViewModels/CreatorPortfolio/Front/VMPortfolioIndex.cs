namespace CraftDailyCorner.ViewModels.CreatorPortfolio.Front
{
    public class VMPortfolioIndex
    {
        public VMPortfolioIndexQuery Query { get; set; } = new();

        public List<VMCreatorPortfolioPublicListItem> Portfolios { get; set; } = new();

        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / Query.PageSize);

        public bool HasPreviousPage => Query.Page > 1;

        public bool HasNextPage => Query.Page < TotalPages;
    }
}

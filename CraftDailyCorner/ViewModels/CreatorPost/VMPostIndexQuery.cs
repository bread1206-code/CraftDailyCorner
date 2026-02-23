namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMPostIndexQuery
    {
        public string? Keyword { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 12;
    }
}

namespace CraftDailyCorner.ViewModels.CreatorPost
{
    public class VMPostIndex
    {
        public VMPostIndexQuery Query { get; set; } = new();

        public List<VMPostListItem> Posts { get; set; } = new();
        public string CreatorID { get; set; } = null!;
        public string CreatorName { get; set; } = null!;
        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / Query.PageSize);

        public bool HasPreviousPage => Query.Page > 1;

        public bool HasNextPage => Query.Page < TotalPages;
    }
}
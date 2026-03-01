namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorIndex
    {
        public VMCreatorIndexQuery Query { get; set; } = new();

        public List<VMCreatorIndexItem> Creators { get; set; } = new();

        public int TotalPages { get; set; }
        public bool HasPreviousPage => Query.Page > 1;
        public bool HasNextPage => Query.Page < TotalPages;
    }

    public class VMCreatorIndexQuery
    {
        public string? Keyword { get; set; }
        public int Page { get; set; } = 1;

        // 你可自行調整預設一頁幾筆
        public int PageSize { get; set; } = 9;
    }
}
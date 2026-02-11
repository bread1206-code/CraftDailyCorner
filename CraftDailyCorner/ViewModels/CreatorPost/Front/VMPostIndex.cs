namespace CraftDailyCorner.ViewModels.CreatorPost.Front
{
    public class VMPostIndex
    {
        // 搜尋條件
        public VMPostIndexQuery Query { get; set; } = new();

        // 顯示結果
        public List<VMCreatorPostPublicListItem> Posts { get; set; } = new();

        // 分頁資訊
        public int TotalCount { get; set; }

        public int TotalPages =>
            (int)Math.Ceiling((double)TotalCount / Query.PageSize);

        public bool HasPreviousPage => Query.Page > 1;

        public bool HasNextPage => Query.Page < TotalPages;
    }
}

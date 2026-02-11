namespace CraftDailyCorner.ViewModels.CreatorPost.Front
{
    public class VMPostIndexQuery
    {
        // 關鍵字
        public string? Keyword { get; set; }

        // 頁碼
        public int Page { get; set; } = 1;

        // 每頁筆數
        public int PageSize { get; set; } = 16;
    }
}

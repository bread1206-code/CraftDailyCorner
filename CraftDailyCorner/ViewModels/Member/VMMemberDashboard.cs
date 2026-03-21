namespace CraftDailyCorner.ViewModels.Member
{
    // 個人中心首頁
    public class VMMemberDashboard
    {
        // 會員識別 
        public string DisplayName { get; set; } = null!;
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        // 訂單相關
        public int PendingPaymentCount { get; set; }   // 待付款
        public int OrderCount { get; set; }            // 進行中
        public int AllOrderCount { get; set; }   // 所有訂單（包含已完成、已取消）

        // 付款
        public int PaymentCount { get; set; }          // 付款紀錄數

        // 社交
        public int FavoriteCount { get; set; }         // 收藏商品
        public int FollowingCount { get; set; }        // 追蹤創作者
        // 狀態用
        public bool IsCreator { get; set; }
        public string? CreatorApplicationStatusCode { get; set; }
        public string? CreatorApplicationId { get; set; }
    }
}
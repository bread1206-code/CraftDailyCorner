namespace CraftDailyCorner.ViewModels.Front
{
    //會員中心首頁
    public class VMMemberDashboard
    {
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public int OrderCount { get; set; }
        public int PendingPaymentCount { get; set; }
        public int CompletedOrderCount { get; set; }
    }
}

namespace CraftDailyCorner.ViewModels.CreatorAnalytics
{
    public class VMCommunityOverview
    {
        public int TotalPosts { get; set; }
        public int PublishedPosts { get; set; }
        public int DraftPosts { get; set; }

        public int TotalComments { get; set; }

        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }

        public decimal TotalRevenue { get; set; }
    }
}

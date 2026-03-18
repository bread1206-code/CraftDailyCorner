namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class ProductReviewSeedRow
    {
        public long ReviewID { get; set; }
        public byte Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string MemberID { get; set; } = null!;
        public string OrderID { get; set; } = null!;
        public string ProductID { get; set; } = null!;
    }
}
namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedTimeHelper
    {
        public static DateTime GetAppliedAt(DateTime memberCreatedAt, int applicationOffsetDays)
        {
            return memberCreatedAt.AddDays(applicationOffsetDays);
        }

        public static DateTime GetReviewedAt(DateTime appliedAt, int reviewOffsetDays)
        {
            return appliedAt.AddDays(reviewOffsetDays);
        }

        public static DateTime GetConfirmedAt(DateTime reviewedAt, int confirmOffsetDays)
        {
            return reviewedAt.AddDays(confirmOffsetDays);
        }

        public static DateTime GetProductCreatedAt(DateTime creatorConfirmedAt, byte sortOrder)
        {
            // 讓同品牌商品分散建立，不要全擠在同一天
            return creatorConfirmedAt
                .AddDays(2 + ((sortOrder - 1) * 4))
                .AddHours((sortOrder * 3) % 24);
        }
    }
}
namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedPaths
    {
        // ================================
        // CSV 資料來源
        // ================================

        public static string MembersCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Members.csv");

        public static string CreatorsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Creators.csv");

        public static string ProductsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Products.csv");

        public static string ProductImagesCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "ProductImages.csv");

        public static string CreatorPostsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "CreatorPosts.csv");

        public static string PostCommentsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "PostComments.csv");

        public static string ReactionsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Reactions.csv");

        public static string FollowsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Follows.csv");

        public static string OrdersCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Orders.csv");

        public static string OrderDetailsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "OrderDetails.csv");

        public static string PaymentsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Payments.csv");

        public static string ShipmentsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "Shipments.csv");

        public static string ProductReviewsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "ProductReviews.csv");
        public static string FavoriteProductsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "Demo", "DataFiles", "FavoriteProducts.csv");
        // ================================
        // Seed 圖片素材
        // ================================

        public static string CreatorBrand =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "CreatorBrand");

        public static string CreatorPortfolioSample =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "CreatorPortfolioSample");

        public static string PostSample =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "PostSample");

        public static string ProductRaw =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "ProductRaw");
    }
}
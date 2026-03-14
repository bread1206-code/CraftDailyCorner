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

        // ================================
        // Seed 圖片素材
        // ================================

        public static string CreatorBrand =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "CreatorBrand");

        public static string CreatorPortfolioSample =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "CreatorPortfolioSample");

        public static string ProductRaw =>
            Path.Combine(Directory.GetCurrentDirectory(),
                "Seed", "SeedAssets", "ProductRaw");
    }
}
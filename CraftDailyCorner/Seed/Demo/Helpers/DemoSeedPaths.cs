namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedPaths
    {
        public static string CreatorBrand =>
            Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "CreatorBrand");

        public static string CreatorPortfolioSample =>
            Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "CreatorPortfolioSample");

        public static string ProductRaw =>
            Path.Combine(Directory.GetCurrentDirectory(), "SeedAssets", "ProductRaw");

        public static string MembersCsv =>
            Path.Combine(Directory.GetCurrentDirectory(), "Seed", "Demo", "DataFiles", "Members.csv");

        public static string CreatorsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(), "Seed", "Demo", "DataFiles", "Creators.csv");

        public static string ProductsCsv =>
            Path.Combine(Directory.GetCurrentDirectory(), "Seed", "Demo", "DataFiles", "Products.csv");

        public static string ProductImagesCsv =>
            Path.Combine(Directory.GetCurrentDirectory(), "Seed", "Demo", "DataFiles", "ProductImages.csv");
    }
}
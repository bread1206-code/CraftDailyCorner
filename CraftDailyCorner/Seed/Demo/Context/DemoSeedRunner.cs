using CraftDailyCorner.Seed.Demo.Sources;

namespace CraftDailyCorner.Seed.Demo.Context
{
    public class DemoSeedContext
    {
        // 來源資料
        public List<MemberSeedRow> Members { get; set; } = new();
        public List<CreatorSeedRow> Creators { get; set; } = new();
        public List<ProductSeedRow> Products { get; set; } = new();
        public List<ProductImageSeedRow> ProductImages { get; set; } = new();

        // 對照資料：MemberID -> CreatorID
        public Dictionary<string, string> MemberToCreatorMap { get; set; } = new();

        // 品牌圖 GUID：CreatorID -> ImageUrl(GUID)
        public Dictionary<string, string> CreatorBrandImageMap { get; set; } = new();

        // 作品範例圖 GUID：CreatorID -> PortfolioSampleUrl(GUID)
        public Dictionary<string, string> CreatorPortfolioSampleMap { get; set; } = new();

        // 創作者確認完成時間：CreatorID -> ConfirmedAt
        public Dictionary<string, DateTime> CreatorConfirmedAtMap { get; set; } = new();

        // 商品建立時間：ProductID -> CreatedAt
        public Dictionary<string, DateTime> ProductCreatedAtMap { get; set; } = new();
    }
}
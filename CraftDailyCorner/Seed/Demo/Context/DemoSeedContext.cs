using CraftDailyCorner.Seed.Demo.Sources;

namespace CraftDailyCorner.Seed.Demo.Context
{
    public class DemoSeedContext
    {
        // ================================
        // 來源資料
        // ================================

        public List<MemberSeedRow> Members { get; set; } = new();
        public List<CreatorSeedRow> Creators { get; set; } = new();
        public List<ProductSeedRow> Products { get; set; } = new();
        public List<ProductImageSeedRow> ProductImages { get; set; } = new();

        public List<CreatorPostSeedRow> CreatorPosts { get; set; } = new();
        public List<PostCommentSeedRow> PostComments { get; set; } = new();
        public List<ReactionSeedRow> Reactions { get; set; } = new();
        public List<FollowCreatorSeedRow> Follows { get; set; } = new();

        public List<OrderSeedRow> Orders { get; set; } = new();
        public List<OrderDetailSeedRow> OrderDetails { get; set; } = new();
        public List<PaymentSeedRow> Payments { get; set; } = new();
        public List<ShipmentSeedRow> Shipments { get; set; } = new();

        // ================================
        // 對照資料
        // ================================

        // MemberID -> CreatorID
        public Dictionary<string, string> MemberToCreatorMap { get; set; } = new();

        // CreatorID -> ImageUrl(GUID)
        public Dictionary<string, string> CreatorBrandImageMap { get; set; } = new();

        // CreatorID -> PortfolioSampleUrl(GUID)
        public Dictionary<string, string> CreatorPortfolioSampleMap { get; set; } = new();

        // CreatorID -> ConfirmedAt
        public Dictionary<string, DateTime> CreatorConfirmedAtMap { get; set; } = new();

        // ProductID -> CreatedAt
        public Dictionary<string, DateTime> ProductCreatedAtMap { get; set; } = new();

        // BrandName -> CreatorID
        public Dictionary<string, string> BrandNameToCreatorMap { get; set; } = new();

        // CreatorID -> MemberID
        public Dictionary<string, string> CreatorToMemberMap { get; set; } = new();

        // CSV PostID -> DB PostID(Guid)
        public Dictionary<string, string> CsvPostIdToDbPostIdMap { get; set; } = new();

        // DB PostID -> CreatedAt
        public Dictionary<string, DateTime> PostCreatedAtMap { get; set; } = new();

        // OrderID -> CreatedAt
        public Dictionary<string, DateTime> OrderCreatedAtMap { get; set; } = new();

        // 品牌代碼(001) -> CreatorID
        public Dictionary<string, string> BrandCodeToCreatorMap { get; set; } = new();

        // CreatorID -> PortfolioID
        public Dictionary<string, string> CreatorPortfolioMap { get; set; } = new();
    }
}
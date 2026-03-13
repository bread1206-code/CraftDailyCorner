namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class ProductSeedRow
    {
        public string ProductID { get; set; } = null!;
        public string CreatorID { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal CostPrice { get; set; }
        public byte StatusID { get; set; }
        public string StockLevelType { get; set; } = null!;
        public int AlertQty { get; set; }
        public byte SortOrder { get; set; }
        public string? CategoryIDs { get; set; }
        public string? TagIDs { get; set; }
    }
}
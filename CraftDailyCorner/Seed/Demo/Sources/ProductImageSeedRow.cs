namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class ProductImageSeedRow
    {
        public string ProductID { get; set; } = null!;
        public string SourceImageFileName { get; set; } = null!;
        public byte SortOrder { get; set; }
        public byte StatusID { get; set; }
    }
}
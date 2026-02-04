namespace CraftDailyCorner.ViewModels.Front
{
    //訂單商品快照
    public class VMProductSnapshot
    {
        public string ProductId { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? CreatorId { get; set; }
        public string? CreatorName { get; set; }
    }
}

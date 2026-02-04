using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        // 統一由 VM 計算，小數無條件捨去
        public decimal SubTotal => Math.Floor(Price * Quantity);
    }
}

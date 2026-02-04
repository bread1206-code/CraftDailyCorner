using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCartItem
    {
        public string ProductID { get; set; } =null!;
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public short Quantity { get; set; }
        public string? ImageUrl { get; set; }

        // 顯示用（暫時）之後修改
        public string DisplaySubTotal =>
        (Price * Quantity).ToString("0");
    }
}

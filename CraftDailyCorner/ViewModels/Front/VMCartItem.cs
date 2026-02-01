using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCartItem
    {
        public string ProductID { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Qty { get; set; }
        public string? ImageUrl { get; set; }
        // 顯示用（暫時）之後修改
        public string DisplaySubTotal =>
        (Price * Qty).ToString("0");
    }
}

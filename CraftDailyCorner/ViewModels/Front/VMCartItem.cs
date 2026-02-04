using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    //表示「購物車中的單一商品項目」，包含 ProductId、商品名稱、價格、數量、小計、圖片等，用在購物車清單顯示
    public class VMCartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }

        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public string? ImageUrl { get; set; }

        // 顯示用單價（無條件捨去）
        public int PriceInt => (int)Math.Floor(Price);

        public decimal SubTotal => Math.Floor(Price * Quantity);
    }
}

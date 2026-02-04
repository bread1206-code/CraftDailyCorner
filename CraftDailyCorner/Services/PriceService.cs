using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;

namespace CraftDailyCorner.Services
{
    // 價格計算服務
    // 所有金額計算都必須經過這裡

    public class PriceService
    {

        // 計算單一商品的小計（無條件捨去）
        public int CalculateSubTotal(decimal unitPrice, int quantity)
        {
            if (quantity <= 0)
                return 0;

            return (int)Math.Floor(unitPrice * quantity);
        }

        // 計算購物車總金額（無條件捨去）
        public int CalculateTotal(IEnumerable<VMCartItem> cartItems)
        {
            int total = 0;

            foreach (var item in cartItems)
            {
                total += CalculateSubTotal(item.Price, item.Quantity);
            }

            return total;
        }
    }
}

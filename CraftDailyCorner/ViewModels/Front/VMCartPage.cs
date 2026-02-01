using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    public class VMCartPage
    {
        public List<VMCartItem> Items { get; set; } = new();
        
        public int TotalQty => Items.Sum(i => i.Qty);
        public decimal TotalAmount => Items.Sum(i => i.Price * i.Qty);

        // 顯示用（暫時）之後修改
        public string DisplayTotalAmount =>
        TotalAmount.ToString("0");
    }
}

using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Order
{
    //Controller 接收用
    //商品一律由 CartService → Checkout Snapshot 產生
    public class VMCreateOrderRequest
    {
        [Required]
        public string CreatorId { get; set; } = null!;

        [Required]
        public string ReceiverName { get; set; } = null!;

        [Required]
        public string ReceiverPhone { get; set; } = null!;

        [Required]
        public string ReceiverAddress { get; set; } = null!;

        // 本次建立訂單時，實際有勾選的商品
        public List<string> SelectedProductIds { get; set; } = new();
    }
}
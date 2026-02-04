using System.ComponentModel.DataAnnotations;

namespace CraftDailyCorner.ViewModels.Front
{
    //Controller 接收用
    //商品一律由 CartService → Checkout Snapshot 產生
    public class VMCreateOrderRequest
    {
        [Required]
        public string ReceiverName { get; set; } = null!;

        [Required]
        public string ReceiverPhone { get; set; } = null!;

        [Required]
        public string ReceiverAddress { get; set; } = null!;
    }
}

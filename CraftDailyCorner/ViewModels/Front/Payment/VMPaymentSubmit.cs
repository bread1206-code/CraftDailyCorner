using Microsoft.Extensions.Hosting;

namespace CraftDailyCorner.ViewModels.Front.Payment
{
    //模擬付款 點「確認付款」POST 用
    public class VMPaymentSubmit
    {
        public string OrderID { get; set; }
        public byte MethodID { get; set; }

        // Mock Payment
        public bool SimulateSuccess { get; set; }
    }
}

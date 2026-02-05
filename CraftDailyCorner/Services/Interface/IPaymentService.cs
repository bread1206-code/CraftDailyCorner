using CraftDailyCorner.ViewModels.Front;

namespace CraftDailyCorner.Services.Interface
{
    public interface IPaymentService
    {
        //模擬付款 POST
        VMPaymentResult CreateMockPayment(VMPaymentSubmit vm);
        //付款頁 GET
        VMPaymentCreate PreparePayment(string orderId);
        //訂單詳情顯示
        List<VMPaymentRecord> GetPaymentsByOrder(string orderId);
    }
}

namespace CraftDailyCorner.ViewModels.Front.Payment
{
    //付款結果顯示用
    public class VMPaymentResult
    {
        public string OrderID { get; set; }

        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; }

        public byte PaymentStatusID { get; set; }
        public string PaymentStatusName { get; set; }

        public DateTime? PaidAt { get; set; }

        // 顯示用
        public string Message { get; set; }
    }
}

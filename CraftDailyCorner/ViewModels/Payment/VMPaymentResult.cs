namespace CraftDailyCorner.ViewModels.Payment
{
    //付款結果顯示用
    public class VMPaymentResult
    {
        public string OrderID { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public string PaymentMethodName { get; set; } = string.Empty;

        public byte PaymentStatusID { get; set; }
        public string PaymentStatusName { get; set; } = string.Empty;

        public DateTime? PaidAt { get; set; }

        // 顯示用
        public string Message { get; set; } = string.Empty;
    }
}

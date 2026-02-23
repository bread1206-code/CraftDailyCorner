namespace CraftDailyCorner.ViewModels.Payment
{
    //付款紀錄列表用
    public class VMPaymentRecord
    {
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }
        public byte AttemptNo { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; } = string.Empty;

        public string MethodName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

namespace CraftDailyCorner.ViewModels.Front.Payment
{
    //付款紀錄列表用
    public class VMPaymentRecord
    {
        public int PaymentID { get; set; }

        public decimal Amount { get; set; }
        public byte AttemptNo { get; set; }

        public byte StatusID { get; set; }
        public string StatusName { get; set; }

        public string MethodName { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

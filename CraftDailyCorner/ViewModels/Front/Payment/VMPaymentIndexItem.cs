namespace CraftDailyCorner.ViewModels.Front.Payment
{
    //付款紀錄列表用
    public class VMPaymentIndexItem
    {
        public string OrderID { get; set; } = null!;

        public decimal Amount { get; set; }

        public string MethodName { get; set; } = null!;

        public string StatusName { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}

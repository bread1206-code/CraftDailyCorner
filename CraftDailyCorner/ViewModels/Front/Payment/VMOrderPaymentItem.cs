namespace CraftDailyCorner.ViewModels.Front.Payment
{
    public class VMOrderPaymentItem
    {
        public decimal Amount { get; set; }
        public string MethodName { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}

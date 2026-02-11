namespace CraftDailyCorner.ViewModels.Payment
{
    public class VMOrderPaymentList
    {
        public string OrderID { get; set; } = null!;
        public List<VMOrderPaymentItem> Payments { get; set; } = new();
    }
}

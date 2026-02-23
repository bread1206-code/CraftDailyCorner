namespace CraftDailyCorner.ViewModels.CreatorPickList
{
    public class VMCreatorPickList
    {
        //本次選擇的訂單清單
        public List<VMPickListOrder> Orders { get; set; } = new();

        //商品總數彙總
        public List<VMPickListSummaryItem> SummaryItems { get; set; } = new();

        //本次列印訂單數量
        public int TotalOrderCount { get; set; }
    }
}

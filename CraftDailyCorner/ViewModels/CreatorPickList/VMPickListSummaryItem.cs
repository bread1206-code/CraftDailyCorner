namespace CraftDailyCorner.ViewModels.CreatorPickList
{
    // 撿貨商品彙總統計
    public class VMPickListSummaryItem
    {
        public string ProductID { get; set; } = null!;

        public string ProductName { get; set; } = null!;

        //此頁所有訂單加總數量
        public int TotalQuantity { get; set; }
    }
}

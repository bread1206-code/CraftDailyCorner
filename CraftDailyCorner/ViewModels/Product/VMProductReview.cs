namespace CraftDailyCorner.ViewModels.Product
{
    public class VMProductReview
    {
        //單一則商品評論資料
        public string MemberName { get; set; } = null!;
        public int Rating { get; set; }      // 1~5
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}

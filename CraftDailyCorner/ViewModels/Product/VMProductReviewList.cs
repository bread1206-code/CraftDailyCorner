namespace CraftDailyCorner.ViewModels.Product
{
    public class VMProductReviewList
    {
        //多筆評論集合，通常包含 List<VMProductReview> 與平均評分、評論總數
        public string ProductID { get; set; } = null!;
        public double AvgRating { get; set; }
        public int TotalCount { get; set; }
        public List<VMProductReview> Reviews { get; set; }= new();
    }
}

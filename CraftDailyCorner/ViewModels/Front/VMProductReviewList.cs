namespace CraftDailyCorner.ViewModels.Front
{
    public class VMProductReviewList
    {
        public string ProductID { get; set; } = null!;
        public double AvgRating { get; set; }
        public int TotalCount { get; set; }
        public List<VMProductReview> Reviews { get; set; }= new();
    }
}

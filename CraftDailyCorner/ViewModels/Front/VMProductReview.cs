namespace CraftDailyCorner.ViewModels.Front
{
    public class VMProductReview
    {
        public string MemberName { get; set; }
        public int Rating { get; set; }      // 1~5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

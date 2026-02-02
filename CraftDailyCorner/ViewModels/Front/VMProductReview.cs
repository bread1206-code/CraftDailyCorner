namespace CraftDailyCorner.ViewModels.Front
{
    public class VMProductReview
    {
        public string MemberName { get; set; } = null!;
        public int Rating { get; set; }      // 1~5
        public string Comment { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}

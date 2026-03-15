namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class PostCommentSeedRow
    {
        // 給 seed 對照用，不直接進 DB
        public string CsvCommentID { get; set; } = null!;

        // 對應 CreatorPosts.csv 的 CsvPostID
        public string CsvPostID { get; set; } = null!;

        public string MemberID { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}
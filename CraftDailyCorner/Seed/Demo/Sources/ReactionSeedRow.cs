namespace CraftDailyCorner.Seed.Demo.Sources
{
    public class ReactionSeedRow
    {
        // 對應 CreatorPosts.csv 的 CsvPostID
        public string CsvPostID { get; set; } = null!;

        public string MemberID { get; set; } = null!;

        // Like / Love / Haha / Wow / Sad / Angry
        public string ReactionType { get; set; } = "Like";
    }
}
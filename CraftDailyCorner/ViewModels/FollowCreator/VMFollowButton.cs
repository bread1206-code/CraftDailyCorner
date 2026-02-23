namespace CraftDailyCorner.ViewModels.FollowCreator
{
    public class VMFollowButton
    {
        public string CreatorID { get; set; } = null!;
        public bool IsFollowing { get; set; }
        public int FollowerCount { get; set; }
    }
}

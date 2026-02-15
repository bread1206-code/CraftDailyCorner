using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.CreatorPost;
using CraftDailyCorner.ViewModels.FollowCreator;
using CraftDailyCorner.ViewModels.Product;

namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorPublicProfile
    {
        public string CreatorID { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string Intro { get; set; } = null!;

        public DateTime StartDate { get; set; }

        public List<VMCreatorPostPublicListItem> LatestPosts { get; set; } = new();

        public List<VMCreatorPortfolioPublicListItem> LatestPortfolios { get; set; } = new();
        public List<VMCreatorProductPublicListItem> LatestProducts { get; set; } = new();
        public VMFollowButton FollowInfo { get; set; } = null!;
    }
}

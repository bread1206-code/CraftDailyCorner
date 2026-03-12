using CraftDailyCorner.ViewModels.CreatorPortfolio.Front;
using CraftDailyCorner.ViewModels.CreatorPost;
using CraftDailyCorner.ViewModels.FollowCreator;
using CraftDailyCorner.ViewModels.Product;

namespace CraftDailyCorner.ViewModels.Creator
{
    public class VMCreatorPublicProfile
    {
        // ===== 基本資料 =====
        public string CreatorID { get; set; } = null!;
        public string BrandName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        public string BrandIntro { get; set; } = null!;
        public DateTime StartDate { get; set; }

        public bool IsOwner { get; set; }
        // ===== UI Helper（可選，但建議）=====
        public string CreatorImagePath =>
            $"/Photos/03CreatorBrand/{CreatorID}/Medium/{ImageUrl}.png";

        public string StartDateText =>
            StartDate.ToString("yyyy/MM/dd");

        // ===== 區塊資料 =====
        public VMFollowButton FollowInfo { get; set; } = null!;

        public List<VMPostListItem> LatestPosts { get; set; } = new();
        public List<VMCreatorPortfolioPublicListItem> LatestPortfolios { get; set; } = new();

        // 近期上架：你的 View 會用到 IsFavorite
        public List<VMCreatorProductPublicListItem> LatestProducts { get; set; } = new();
    }
}
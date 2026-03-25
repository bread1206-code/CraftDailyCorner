using System;

namespace CraftDailyCorner.ViewModels.Member
{
    // 我的追蹤 - 創作者卡片 ViewModel
    public class VMFollowingCreatorCard
    {
        // 創作者基本資訊

        // 創作者ID
        public string CreatorId { get; set; } = null!;

        // 品牌名稱
        public string BrandName { get; set; } = null!;

        // 創作者 Logo 圖片 Key
        public string? CreatorLogo { get; set; }

        // 創作者首頁連結
        public string CreatorProfileUrl =>
            $"/Creator/Profile/{CreatorId}";

        // 創作者品牌圖路徑
        public string CreatorLogoPath =>
            string.IsNullOrWhiteSpace(CreatorLogo)
                ? $"/Photos/03CreatorBrand/{CreatorId}/Medium/default.webp"
                : $"/Photos/03CreatorBrand/{CreatorId}/Medium/{CreatorLogo}.webp";

        // 最近新增商品

        public string? LatestProductId { get; set; }

        public string? LatestProductName { get; set; }

        public string? LatestProductImage { get; set; }

        public string? LatestProductUrl =>
            LatestProductId == null
                ? null
                : $"/Products/Detail/{LatestProductId}";

        public string LatestProductImagePath =>
            string.IsNullOrWhiteSpace(LatestProductImage)
                ? "/images/no-image.webp"
                : $"/Photos/04ProductImage/{CreatorId}/Medium/{LatestProductImage}.webp";

        // 最近新增日誌

        public string? LatestPostId { get; set; }

        public string? LatestPostTitle { get; set; }

        public string? LatestPostImage { get; set; }

        public string? LatestPostUrl =>
            LatestPostId == null
                ? null
                : $"/Post/Detail/{LatestPostId}";

        public string LatestPostImagePath =>
            string.IsNullOrWhiteSpace(LatestPostImage)
                ? "/images/no-image.webp"
                : $"/Photos/05CreatorPost/{CreatorId}/Medium/{LatestPostImage}.webp";

        // 最近新增作品集

        public string? LatestPortfolioId { get; set; }

        public string? LatestPortfolioTitle { get; set; }

        public string? LatestPortfolioImage { get; set; }

        public string? LatestPortfolioUrl =>
            LatestPortfolioId == null
                ? null
                : $"/Portfolio/Detail/{LatestPortfolioId}";

        public string LatestPortfolioImagePath =>
            string.IsNullOrWhiteSpace(LatestPortfolioImage)
                ? "/images/no-image.webp"
                : $"/Photos/06Portfolio/{CreatorId}/Medium/{LatestPortfolioImage}.webp";
    }
}
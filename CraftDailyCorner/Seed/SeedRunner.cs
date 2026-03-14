using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Datas;
using CraftDailyCorner.Services;

namespace CraftDailyCorner.Seed
{
    public class SeedRunner
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;
        private readonly SeedMember _seedMember;
        private readonly SeedPrivacy _seedPrivacy;
        private readonly SeedRole _seedRole;
        private readonly SeedMemberRole _seedMemberRole;
        private readonly SeedMemberRoleHistory _seedMemberRoleHistory;
        private readonly SeedCreatorApplication _seedCreatorApplication;
        private readonly SeedCreatorProfile _seedCreatorProfile;
        private readonly SeedProduct _seedProduct;
        private readonly SeedProductImage _seedProductImage;
        private readonly SeedCategory _seedCategory;
        private readonly SeedTag _seedTag;
        private readonly SeedProductRelation _seedProductRelation;
        private readonly SeedCart _seedCart;
        private readonly SeedCartItem _seedCartItem;
        private readonly SeedOrder _seedOrder;
        private readonly SeedOrderDetail _seedOrderDetail;
        private readonly SeedPayment _seedPayment;
        private readonly SeedShipment _seedShipment;
        private readonly SeedFavoriteProduct _seedFavoriteProduct;
        private readonly SeedFollowCreator _seedFollowCreator;
        private readonly SeedProductReview _seedProductReview;
        private readonly SeedMessageThread _seedMessageThread;
        private readonly SeedMessage _seedMessage;
        private readonly SeedAutoReplyTemplate _seedAutoReplyTemplate;
        private readonly SeedCreatorPost _seedCreatorPost;
        private readonly SeedPostComment _seedPostComment;
        private readonly SeedInventory _seedInventory;
        private readonly SeedInventoryAlert _seedInventoryAlert;
        private readonly SeedPlatformAnnouncement _seedPlatformAnnouncement;
        private readonly SeedHomepageBanner _seedHomepageBanner;
        private readonly SeedPlatformSetting _seedPlatformSetting;
        private readonly SeedPortfolio _seedPortfolio;
        private readonly SeedPortfolioItem _seedPortfolioItem;
        private readonly SeedNotificationPreference _seedNotificationPreference;
        private readonly SeedNotificationEvent _seedNotificationEvent;
        private readonly SeedMemberStatus _seedMemberStatus;
        private readonly SeedCreatorApplicationStatus _seedCreatorApplicationStatus;
        private readonly SeedCreatorProfileStatus _seedCreatorProfileStatus;
        private readonly SeedProductStatus _seedProductStatus;
        private readonly SeedProductImageStatus _seedProductImageStatus;
        private readonly SeedOrderStatus _seedOrderStatus;
        private readonly SeedPaymentMethod _seedPaymentMethod;
        private readonly SeedPaymentStatus _seedPaymentStatus;
        private readonly SeedShipmentStatus _seedShipmentStatus;
        private readonly SeedCreatorPostStatus _seedCreatorPostStatus;
        private readonly SeedPlatformAnnouncementStatus _seedPlatformAnnouncementStatus;
        private readonly SeedHomepageBannerStatus _seedHomepageBannerStatus;
        private readonly SeedPlatformSettingCategory _seedPlatformSettingCategory;
        private readonly SeedReportStatus _seedPostCommentReportStatus;
        private readonly SeedPortfolioStatus _seedPortfolioStatus;


        private readonly Dictionary<string, List<ImageSizeOption>> _folderSizeMapping =
            new()
            {
                { "01Member", ImageSizePresets.Member },
                { "02CreatorApplication", ImageSizePresets.CreatorApplication },
                { "03CreatorBrand", ImageSizePresets.Creator },
                { "04ProductImage", ImageSizePresets.Product },
                { "05CreatorPost", ImageSizePresets.Post },
                { "06Portfolio", ImageSizePresets.Portfolio },
                { "07Logo", ImageSizePresets.Logo },
                { "08HomepageBanner", ImageSizePresets.HomepageBanner }
            };



        public SeedRunner(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService,
            SeedMember seedMember,
            SeedPrivacy seedPrivacy,
            SeedRole seedRole,
            SeedMemberRole seedMemberRole,
            SeedMemberRoleHistory seedMemberRoleHistory,
            SeedCreatorApplication seedCreatorApplication,
            SeedCreatorProfile seedCreatorProfile,
            SeedProduct seedProduct,
            SeedProductImage seedProductImage,
            SeedCategory seedCategory,
            SeedTag seedTag,
            SeedProductRelation seedProductRelation,
            SeedCart seedCart,
            SeedCartItem seedCartItem,
            SeedOrder seedOrder,
            SeedOrderDetail seedOrderDetail,
            SeedPayment seedPayment,
            SeedShipment seedShipment,
            SeedFavoriteProduct seedFavoriteProduct,
            SeedFollowCreator seedFollowCreator,
            SeedProductReview seedProductReview,
            SeedMessageThread seedMessageThread,
            SeedMessage seedMessage,
            SeedAutoReplyTemplate seedAutoReplyTemplate,
            SeedCreatorPost seedCreatorPost,
            SeedPostComment seedPostComment,
            SeedInventory seedInventory,
            SeedInventoryAlert seedInventoryAlert,
            SeedPlatformAnnouncement seedPlatformAnnouncement,
            SeedHomepageBanner seedHomepageBanner,
            SeedPlatformSetting seedPlatformSetting,
            SeedPortfolio seedPortfolio,
            SeedPortfolioItem seedPortfolioItem,
            SeedNotificationPreference seedNotificationPreference,
            SeedNotificationEvent seedNotificationEvent,
            SeedMemberStatus seedMemberStatus,
            SeedCreatorApplicationStatus seedCreatorApplicationStatus,
            SeedCreatorProfileStatus seedCreatorProfileStatus,
            SeedProductStatus seedProductStatus,
            SeedProductImageStatus seedProductImageStatus,
            SeedOrderStatus seedOrderStatus,
            SeedPaymentMethod seedPaymentMethod,
            SeedPaymentStatus seedPaymentStatus,
            SeedShipmentStatus seedShipmentStatus,
            SeedCreatorPostStatus seedCreatorPostStatus,
            SeedPlatformAnnouncementStatus seedPlatformAnnouncementStatus,
            SeedHomepageBannerStatus seedHomepageBannerStatus,
            SeedPlatformSettingCategory seedPlatformSettingCategory,
            SeedReportStatus seedPostCommentReportStatus,
            SeedPortfolioStatus seedPortfolioStatus
        )
        {
            _context = context;
            _imageUploadService = imageUploadService;
            _seedMember = seedMember;
            _seedPrivacy = seedPrivacy;
            _seedRole = seedRole;
            _seedMemberRole = seedMemberRole;
            _seedMemberRoleHistory = seedMemberRoleHistory;
            _seedCreatorApplication = seedCreatorApplication;
            _seedCreatorProfile = seedCreatorProfile;
            _seedProduct = seedProduct;
            _seedProductImage = seedProductImage;
            _seedCategory = seedCategory;
            _seedTag = seedTag;
            _seedProductRelation = seedProductRelation;
            _seedCart = seedCart;
            _seedCartItem = seedCartItem;
            _seedOrder = seedOrder;
            _seedOrderDetail = seedOrderDetail;
            _seedPayment = seedPayment;
            _seedShipment = seedShipment;
            _seedFavoriteProduct = seedFavoriteProduct;
            _seedFollowCreator = seedFollowCreator;
            _seedProductReview = seedProductReview;
            _seedMessageThread = seedMessageThread;
            _seedMessage = seedMessage;
            _seedAutoReplyTemplate = seedAutoReplyTemplate;
            _seedCreatorPost = seedCreatorPost;
            _seedPostComment = seedPostComment;
            _seedInventory = seedInventory;
            _seedInventoryAlert = seedInventoryAlert;
            _seedPlatformAnnouncement = seedPlatformAnnouncement;
            _seedHomepageBanner = seedHomepageBanner;
            _seedPlatformSetting = seedPlatformSetting;
            _seedPortfolio = seedPortfolio;
            _seedPortfolioItem = seedPortfolioItem;
            _seedNotificationPreference = seedNotificationPreference;
            _seedNotificationEvent = seedNotificationEvent;
            _seedMemberStatus = seedMemberStatus;
            _seedCreatorApplicationStatus = seedCreatorApplicationStatus;
            _seedCreatorProfileStatus = seedCreatorProfileStatus;
            _seedProductStatus = seedProductStatus;
            _seedProductImageStatus = seedProductImageStatus;
            _seedOrderStatus = seedOrderStatus;
            _seedPaymentMethod = seedPaymentMethod;
            _seedPaymentStatus = seedPaymentStatus;
            _seedShipmentStatus = seedShipmentStatus;
            _seedCreatorPostStatus = seedCreatorPostStatus;
            _seedPlatformAnnouncementStatus = seedPlatformAnnouncementStatus;
            _seedHomepageBannerStatus = seedHomepageBannerStatus;
            _seedPlatformSettingCategory = seedPlatformSettingCategory;
            _seedPostCommentReportStatus = seedPostCommentReportStatus;
            _seedPortfolioStatus = seedPortfolioStatus;
        }

        public void Run()
        {
            if (!_context.Members.Any())
            {
                // 1️ 準備 GUID
            var memberGuids = GenerateGuids(1);
            //var creatorApplicationGuids = GenerateGuids(3);
            //var creatorBrandGuids = GenerateGuids(2);
            //var productImageGuids = GenerateGuids(6);
            //var creatorPostGuids = GenerateGuids(7);
            //var seedPortfolioItemGuids = GenerateGuids(2);
            //var prtfolioGuids = GenerateGuids(1);
            var homepageBannerGuids = GenerateGuids(6);
            Console.WriteLine("準備 GUID 完成");

            
            // 2 上傳圖片
            UploadImages(memberGuids, "01Member");
            //UploadImages(creatorApplicationGuids, "02CreatorApplication");
            //UploadImages(creatorBrandGuids, "03CreatorBrand");
            //UploadImages(productImageGuids, "04ProductImage");
            //UploadImages(creatorPostGuids, "05CreatorPost");
            //UploadImages(seedPortfolioItemGuids, "06Portfolio");
            UploadImages(homepageBannerGuids, "08HomepageBanner");
            UploadImages();// 上傳預設會員圖片、預設Logo圖片
            Console.WriteLine("上傳圖片 完成");


                // 3 更新Seed資料
            _seedMemberStatus.Run();
            _seedCreatorApplicationStatus.Run();
            _seedPortfolioStatus.Run();
            _seedCreatorProfileStatus.Run();
            _seedProductStatus.Run();
            _seedProductImageStatus.Run();
            _seedOrderStatus.Run();
            _seedPaymentStatus.Run();
            _seedShipmentStatus.Run();
            _seedCreatorPostStatus.Run();
            _seedPlatformAnnouncementStatus.Run();
            _seedHomepageBannerStatus.Run();
            _seedPlatformSettingCategory.Run();
            _seedPostCommentReportStatus.Run();
            _seedPaymentMethod.Run();
            _seedRole.Run();
            _seedTag.Run();
            _seedCategory.Run();
            

            _seedMember.Run(memberGuids);
            _seedPrivacy.Run();
            _seedMemberRole.Run();
            _seedMemberRoleHistory.Run();
            //_seedCreatorApplication.Run(creatorApplicationGuids);
            //_seedCreatorProfile.Run(creatorBrandGuids);
            //_seedProduct.Run();
            //_seedProductImage.Run(productImageGuids);
            //_seedProductRelation.Run();
            _seedCart.Run();
            //_seedCartItem.Run();
            //_seedOrder.Run();
            //_seedOrderDetail.Run();
            //_seedPayment.Run();
            //_seedShipment.Run();
            //_seedFavoriteProduct.Run();
            //_seedFollowCreator.Run();
            //_seedProductReview.Run();
            //_seedMessageThread.Run();
            //_seedMessage.Run();
            //_seedAutoReplyTemplate.Run();
            //_seedCreatorPost.Run(creatorPostGuids);
            //_seedPostComment.Run(creatorPostGuids);
            //_seedInventory.Run();
            //_seedInventoryAlert.Run();
            _seedPlatformAnnouncement.Run();
            _seedHomepageBanner.Run(homepageBannerGuids);
            _seedPlatformSetting.Run();
            //_seedPortfolio.Run(prtfolioGuids);
            //_seedPortfolioItem.Run(prtfolioGuids, seedPortfolioItemGuids);
            //_seedNotificationPreference.Run();
            //_seedNotificationEvent.Run();

            Console.WriteLine("更新Seed資料 完成");
            
            }
        }
        private void UploadImages(string[] guids, string seedFolder)
        {
            string seedPhotoPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "Seed",
                "SeedPhotos",
                seedFolder
            );

            var files = Directory.GetFiles(seedPhotoPath)
                        .OrderBy(f => f) // 很重要，確保順序穩定
                        .ToArray();
            if (files.Length < guids.Length)
                throw new Exception($"{seedFolder} 圖片數量不足");

            // 根據 seedFolder 取得對應尺寸設定
            var sizes = _folderSizeMapping.ContainsKey(seedFolder)
                ? _folderSizeMapping[seedFolder]
                : ImageSizePresets.Member; // 預設使用 Member

            for (int i = 0; i < guids.Length; i++)
            {
                _imageUploadService.UploadFromSeed(
                    seedFolder: seedFolder,
                    sourceFile: files[i],
                    fileNameWithoutExt: guids[i],
                    sizes: sizes
                );
            }
        }
        private void UploadImages()
        {
            var sizes = _folderSizeMapping.ContainsKey("01Member")
                ? _folderSizeMapping["01Member"]
                : ImageSizePresets.Member; // 預設使用 Member
            _imageUploadService.UploadFromSeed(
                    seedFolder: "01Member",
                    sourceFile: "Seed/SeedPhotos/01Member/default.png",
                    fileNameWithoutExt: "default",
                    sizes: sizes
                );
            var sizesLogo = _folderSizeMapping.ContainsKey("07Logo")
                ? _folderSizeMapping["07Logo"]
                : ImageSizePresets.Logo;
            _imageUploadService.UploadFromSeed(
                    seedFolder: "07Logo",
                    sourceFile: "Seed/SeedPhotos/07Logo/platformLogo.png",
                    fileNameWithoutExt: "platformLogo",
                    sizes: sizesLogo
                );
        }

        private string[] GenerateGuids(int count)
        {
            var guids = new string[count];
            for (int i = 0; i < count; i++)
                guids[i] = Guid.NewGuid().ToString();

            return guids;
        }
    }
}

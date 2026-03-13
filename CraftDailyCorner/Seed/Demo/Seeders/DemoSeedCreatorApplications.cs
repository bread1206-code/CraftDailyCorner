using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedCreatorApplications
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedCreatorApplications(
            CraftDailyCornerContext context,
            DemoSeedImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            if (seedContext.Creators == null || !seedContext.Creators.Any())
                throw new Exception("DemoSeedContext.Creators 沒有資料");

            if (_context.CreatorApplications.Any())
                return;

            var orderedPortfolioFiles = _imageHelper.GetOrderedFiles(DemoSeedPaths.CreatorPortfolioSample);

            var applications = new List<CreatorApplication>();

            for (int i = 0; i < seedContext.Creators.Count; i++)
            {
                var row = seedContext.Creators[i];

                var member = seedContext.Members.FirstOrDefault(x => x.MemberID == row.MemberID)
                    ?? throw new Exception($"找不到對應會員：{row.MemberID}");

                var appliedAt = DemoSeedTimeHelper.GetAppliedAt(member.CreatedAt, row.ApplicationOffsetDays);
                var reviewedAt = DemoSeedTimeHelper.GetReviewedAt(appliedAt, row.ReviewOffsetDays);
                var confirmedAt = DemoSeedTimeHelper.GetConfirmedAt(reviewedAt, row.ConfirmOffsetDays);

                var sourceImagePath = _imageHelper.GetFileByIndex(
                    orderedFiles: orderedPortfolioFiles,
                    index: i,
                    folderName: "CreatorPortfolioSample");

                var portfolioSampleGuid = _imageHelper.UploadCreatorPortfolioSampleImage(
                    sourceFilePath: sourceImagePath,
                    creatorId: row.CreatorID);

                applications.Add(new CreatorApplication
                {
                    BrandName = row.BrandName,
                    BrandIntro = row.BrandIntro,
                    PortfolioSampleUrl = portfolioSampleGuid,
                    StartDate = row.StartDate,
                    StatusID = 4,
                    AppliedAt = appliedAt,
                    ReviewedAt = reviewedAt,
                    ReviewNote = "審核通過，已完成創作者資格確認。",
                    MemberID = row.MemberID,
                    ReviewedBy = "M0000001"
                });

                seedContext.MemberToCreatorMap[row.MemberID] = row.CreatorID;
                seedContext.CreatorPortfolioSampleMap[row.CreatorID] = portfolioSampleGuid;
                seedContext.CreatorConfirmedAtMap[row.CreatorID] = confirmedAt;
            }

            _context.CreatorApplications.AddRange(applications);
            _context.SaveChanges();
        }
    }
}
using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedCreatorProfiles
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedCreatorProfiles(
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

            if (seedContext.Creators == null || !seedContext.Creators.Any())
                throw new Exception("DemoSeedContext.Creators 沒有資料");

            var existingCreatorProfiles = _context.CreatorProfiles
                .Select(x => new
                {
                    x.CreatorID,
                    x.MemberID,
                    x.BrandName,
                    x.ImageUrl,
                    x.CreatedAt
                })
                .ToList();

            var existingCreatorIdMap = existingCreatorProfiles
                .ToDictionary(x => x.CreatorID, x => x);

            var orderedBrandFiles = _imageHelper.GetOrderedFiles(DemoSeedPaths.CreatorBrand);

            var creatorProfiles = new List<CreatorProfile>();

            for (int i = 0; i < seedContext.Creators.Count; i++)
            {
                var row = seedContext.Creators[i];

                var creatorId = row.CreatorID.Trim();
                var memberId = row.MemberID.Trim();
                var brandName = row.BrandName.Trim();

                if (existingCreatorIdMap.TryGetValue(creatorId, out var existingProfile))
                {
                    seedContext.CreatorBrandImageMap[creatorId] = existingProfile.ImageUrl;
                    seedContext.MemberToCreatorMap[memberId] = creatorId;
                    seedContext.BrandNameToCreatorMap[brandName] = creatorId;
                    seedContext.CreatorToMemberMap[creatorId] = memberId;

                    // 這裡很重要：補 ConfirmedAt fallback
                    if (!seedContext.CreatorConfirmedAtMap.ContainsKey(creatorId))
                    {
                        seedContext.CreatorConfirmedAtMap[creatorId] = existingProfile.CreatedAt;
                    }

                    continue;
                }

                DateTime confirmedAt;
                if (!seedContext.CreatorConfirmedAtMap.TryGetValue(creatorId, out confirmedAt))
                {
                    // 如果前面的申請 Seeder 沒補到，就用 StartDate 當 fallback
                    confirmedAt = row.StartDate;
                    seedContext.CreatorConfirmedAtMap[creatorId] = confirmedAt;
                }

                var sourceImagePath = _imageHelper.GetFileByIndex(
                    orderedFiles: orderedBrandFiles,
                    index: i,
                    folderName: "CreatorBrand");

                var brandImageGuid = _imageHelper.UploadCreatorBrandImage(
                    sourceFilePath: sourceImagePath,
                    creatorId: creatorId);

                creatorProfiles.Add(new CreatorProfile
                {
                    CreatorID = creatorId,
                    ImageUrl = brandImageGuid,
                    BrandName = brandName,
                    BrandIntro = row.BrandIntro,
                    StartDate = row.StartDate,
                    BankCode = row.BankCode,
                    BankAccount = row.BankAccount,
                    StatusID = 1,
                    CreatedAt = confirmedAt,
                    UpdatedAt = confirmedAt,
                    MemberID = memberId
                });

                seedContext.CreatorBrandImageMap[creatorId] = brandImageGuid;
                seedContext.MemberToCreatorMap[memberId] = creatorId;
                seedContext.BrandNameToCreatorMap[brandName] = creatorId;
                seedContext.CreatorToMemberMap[creatorId] = memberId;
            }

            if (creatorProfiles.Any())
            {
                _context.CreatorProfiles.AddRange(creatorProfiles);
                _context.SaveChanges();
            }
        }
    }
}
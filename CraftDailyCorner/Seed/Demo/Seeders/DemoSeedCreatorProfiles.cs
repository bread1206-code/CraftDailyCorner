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

            var existingCreatorIds = _context.CreatorProfiles
                .Select(x => x.CreatorID)
                .ToHashSet();

            var orderedBrandFiles = _imageHelper.GetOrderedFiles(DemoSeedPaths.CreatorBrand);

            var creatorProfiles = new List<CreatorProfile>();

            for (int i = 0; i < seedContext.Creators.Count; i++)
            {
                var row = seedContext.Creators[i];

                if (existingCreatorIds.Contains(row.CreatorID))
                    continue;

                if (!seedContext.CreatorConfirmedAtMap.TryGetValue(row.CreatorID, out var confirmedAt))
                    throw new Exception($"找不到 CreatorConfirmedAt：{row.CreatorID}");

                var sourceImagePath = _imageHelper.GetFileByIndex(
                    orderedFiles: orderedBrandFiles,
                    index: i,
                    folderName: "CreatorBrand");

                var brandImageGuid = _imageHelper.UploadCreatorBrandImage(
                    sourceFilePath: sourceImagePath,
                    creatorId: row.CreatorID);

                creatorProfiles.Add(new CreatorProfile
                {
                    CreatorID = row.CreatorID,
                    ImageUrl = brandImageGuid,
                    BrandName = row.BrandName,
                    BrandIntro = row.BrandIntro,
                    StartDate = row.StartDate,
                    BankCode = row.BankCode,
                    BankAccount = row.BankAccount,
                    StatusID = 1,
                    CreatedAt = confirmedAt,
                    UpdatedAt = confirmedAt,
                    MemberID = row.MemberID
                });

                seedContext.CreatorBrandImageMap[row.CreatorID] = brandImageGuid;
                seedContext.MemberToCreatorMap[row.MemberID] = row.CreatorID;
            }

            if (creatorProfiles.Any())
            {
                _context.CreatorProfiles.AddRange(creatorProfiles);
                _context.SaveChanges();
            }
        }
    }
}
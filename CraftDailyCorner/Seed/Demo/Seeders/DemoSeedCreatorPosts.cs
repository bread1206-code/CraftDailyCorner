using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;
using CraftDailyCorner.Seed.Demo.Helpers;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedCreatorPosts
    {
        private readonly CraftDailyCornerContext _context;
        private readonly DemoSeedImageHelper _imageHelper;

        public DemoSeedCreatorPosts(
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

            if (seedContext.CreatorPosts == null || !seedContext.CreatorPosts.Any())
                throw new Exception("DemoSeedContext.CreatorPosts 沒有資料");

            var existingPosts = _context.CreatorPosts
                .Select(x => new
                {
                    x.PostID,
                    x.CreatorID,
                    x.Title,
                    x.CreatedAt
                })
                .ToList();

            var existingPostMap = existingPosts
                .ToDictionary(
                    x => $"{x.CreatorID}|||{x.Title.Trim()}",
                    x => x);

            var orderedPostFiles = _imageHelper.GetOrderedFiles(DemoSeedPaths.PostSample);

            var creatorPosts = new List<CreatorPost>();

            // 每位創作者各自重新編號
            var creatorPostCounter = new Dictionary<string, int>();

            for (int i = 0; i < seedContext.CreatorPosts.Count; i++)
            {
                var row = seedContext.CreatorPosts[i];
                var brandName = row.BrandName.Trim();
                var title = row.Title.Trim();

                if (!seedContext.BrandNameToCreatorMap.TryGetValue(brandName, out var creatorId))
                    throw new Exception($"CreatorPosts.csv 找不到對應品牌：{brandName}");

                var key = $"{creatorId}|||{title}";

                // 如果 DB 已存在，也要把 map 補回 seedContext
                if (existingPostMap.TryGetValue(key, out var existingPost))
                {
                    seedContext.CsvPostIdToDbPostIdMap[row.CsvPostID] = existingPost.PostID;
                    seedContext.PostCreatedAtMap[existingPost.PostID] = existingPost.CreatedAt;
                    continue;
                }

                if (!seedContext.CreatorConfirmedAtMap.TryGetValue(creatorId, out var confirmedAt))
                    throw new Exception($"找不到 CreatorConfirmedAt：{creatorId}");

                if (!creatorPostCounter.ContainsKey(creatorId))
                    creatorPostCounter[creatorId] = 0;

                creatorPostCounter[creatorId]++;

                var postIndex = creatorPostCounter[creatorId];

                var postId = Guid.NewGuid().ToString();

                var sourceImagePath = _imageHelper.GetFileByIndex(
                    orderedFiles: orderedPostFiles,
                    index: i,
                    folderName: "PostSample");

                var imageGuid = _imageHelper.UploadCreatorPostImage(
                    sourceFilePath: sourceImagePath,
                    creatorId: creatorId);

                // 每位 Creator 依自己的發文數往後 +1 天
                var createdAt = confirmedAt.AddDays(0 + postIndex);
                var updatedAt = createdAt;

                var visibility = ParseVisibility(row.Visibility);
                var statusId = ParseStatusId(row.Status);

                creatorPosts.Add(new CreatorPost
                {
                    PostID = postId,
                    Title = title,
                    Content = row.Content.Replace("<br>", "\n"),
                    ImageUrl = imageGuid,
                    Visibility = visibility,
                    StatusID = statusId,
                    CreatedAt = createdAt,
                    UpdatedAt = updatedAt,
                    CreatorID = creatorId
                });

                seedContext.CsvPostIdToDbPostIdMap[row.CsvPostID] = postId;
                seedContext.PostCreatedAtMap[postId] = createdAt;
            }

            if (creatorPosts.Any())
            {
                _context.CreatorPosts.AddRange(creatorPosts);
                _context.SaveChanges();
            }
        }

        private static CreatorVisibility ParseVisibility(string value)
        {
            var text = value?.Trim().ToLowerInvariant();

            return text switch
            {
                "public" => CreatorVisibility.Public,
                "followers" => CreatorVisibility.Followers,
                "follower" => CreatorVisibility.Followers,
                "private" => CreatorVisibility.Private,
                _ => CreatorVisibility.Public
            };
        }

        private static byte ParseStatusId(string value)
        {
            var text = value?.Trim().ToLowerInvariant();

            return text switch
            {
                "active" => 1,
                "suspended" => 2,
                "deleted" => 3,
                _ => 1
            };
        }
    }
}
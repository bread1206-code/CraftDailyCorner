using CraftDailyCorner.Seed.Demo.Sources;
using System.Text;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class CreatorPostSeedLoader
    {
        public List<CreatorPostSeedRow> LoadPosts(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 CreatorPosts.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<CreatorPostSeedRow>();

            var rows = new List<CreatorPostSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 7)
                    throw new Exception($"CreatorPosts.csv 欄位數不足：{line}");

                rows.Add(new CreatorPostSeedRow
                {
                    CsvPostID = parts[0].Trim(),
                    BrandName = parts[1].Trim(),
                    Title = parts[2].Trim(),
                    Content = parts[3].Trim(),
                    CoverImage = parts[4].Trim(),
                    Visibility = parts[5].Trim(),
                    Status = parts[6].Trim()
                });
            }

            return rows;
        }

        public List<PostCommentSeedRow> LoadComments(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 PostComments.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<PostCommentSeedRow>();

            var rows = new List<PostCommentSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 4)
                    throw new Exception($"PostComments.csv 欄位數不足：{line}");

                rows.Add(new PostCommentSeedRow
                {
                    CsvCommentID = parts[0].Trim(),
                    CsvPostID = parts[1].Trim(),
                    MemberID = parts[2].Trim(),
                    Content = parts[3].Trim()
                });
            }

            return rows;
        }

        public List<ReactionSeedRow> LoadReactions(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Reactions.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<ReactionSeedRow>();

            var rows = new List<ReactionSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 2)
                    throw new Exception($"Reactions.csv 欄位數不足：{line}");

                var reactionType = parts.Length >= 3
                    ? parts[2].Trim()
                    : "Like";

                rows.Add(new ReactionSeedRow
                {
                    CsvPostID = parts[0].Trim(),
                    MemberID = parts[1].Trim(),
                    ReactionType = string.IsNullOrWhiteSpace(reactionType) ? "Like" : reactionType
                });
            }

            return rows;
        }

        public List<FollowCreatorSeedRow> LoadFollows(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Follows.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<FollowCreatorSeedRow>();

            var rows = new List<FollowCreatorSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 2)
                    throw new Exception($"Follows.csv 欄位數不足：{line}");

                rows.Add(new FollowCreatorSeedRow
                {
                    BrandName = parts[0].Trim(),
                    MemberID = parts[1].Trim()
                });
            }

            return rows;
        }
    }
}
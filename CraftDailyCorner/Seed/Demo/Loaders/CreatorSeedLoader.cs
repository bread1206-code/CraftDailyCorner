using CraftDailyCorner.Seed.Demo.Sources;
using System.Globalization;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class CreatorSeedLoader
    {
        public List<CreatorSeedRow> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Creators.csv", csvPath);

            var lines = File.ReadAllLines(csvPath);

            if (lines.Length <= 1)
                return new List<CreatorSeedRow>();

            var rows = new List<CreatorSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 10)
                    throw new Exception($"Creators.csv 欄位數不足：{line}");

                rows.Add(new CreatorSeedRow
                {
                    MemberID = parts[0].Trim(),
                    CreatorID = parts[1].Trim(),
                    BrandName = parts[2].Trim(),
                    BrandIntro = parts[3].Trim(),
                    StartDate = DateTime.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                    ApplicationOffsetDays = int.Parse(parts[5].Trim()),
                    ReviewOffsetDays = int.Parse(parts[6].Trim()),
                    ConfirmOffsetDays = int.Parse(parts[7].Trim()),
                    BankCode = ParseNullableString(parts[8]),
                    BankAccount = ParseNullableString(parts[9])
                });
            }

            return rows;
        }

        private static string? ParseNullableString(string value)
        {
            var text = value?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }
    }
}
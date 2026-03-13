using CraftDailyCorner.Seed.Demo.Sources;
using System.Globalization;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class ProductSeedLoader
    {
        public List<ProductSeedRow> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Products.csv", csvPath);

            var lines = File.ReadAllLines(csvPath);

            if (lines.Length <= 1)
                return new List<ProductSeedRow>();

            var rows = new List<ProductSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 12)
                    throw new Exception($"Products.csv 欄位數不足：{line}");

                rows.Add(new ProductSeedRow
                {
                    ProductID = parts[0].Trim(),
                    CreatorID = parts[1].Trim(),
                    ProductName = parts[2].Trim(),
                    Description = parts[3].Trim(),
                    Price = decimal.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                    CostPrice = decimal.Parse(parts[5].Trim(), CultureInfo.InvariantCulture),
                    StatusID = byte.Parse(parts[6].Trim()),
                    StockLevelType = parts[7].Trim(),
                    AlertQty = int.Parse(parts[8].Trim()),
                    SortOrder = byte.Parse(parts[9].Trim()),
                    CategoryIDs = ParseNullableString(parts[10]),
                    TagIDs = ParseNullableString(parts[11])
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
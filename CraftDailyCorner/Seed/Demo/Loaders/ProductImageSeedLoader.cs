using CraftDailyCorner.Seed.Demo.Sources;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class ProductImageSeedLoader
    {
        public List<ProductImageSeedRow> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 ProductImages.csv", csvPath);

            var lines = File.ReadAllLines(csvPath);

            if (lines.Length <= 1)
                return new List<ProductImageSeedRow>();

            var rows = new List<ProductImageSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 4)
                    throw new Exception($"ProductImages.csv 欄位數不足：{line}");

                rows.Add(new ProductImageSeedRow
                {
                    ProductID = parts[0].Trim(),
                    SourceImageFileName = parts[1].Trim(),
                    SortOrder = byte.Parse(parts[2].Trim()),
                    StatusID = byte.Parse(parts[3].Trim())
                });
            }

            return rows;
        }
    }
}
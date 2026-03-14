using CraftDailyCorner.Seed.Demo.Sources;
using System.Globalization;
using System.Text;

namespace CraftDailyCorner.Seed.Demo.Loaders
{
    public class MemberSeedLoader
    {
        public List<MemberSeedRow> Load(string csvPath)
        {
            if (string.IsNullOrWhiteSpace(csvPath))
                throw new ArgumentException("CSV 路徑不可為空");

            if (!File.Exists(csvPath))
                throw new FileNotFoundException("找不到 Members.csv", csvPath);

            var lines = File.ReadAllLines(csvPath, Encoding.UTF8);

            if (lines.Length <= 1)
                return new List<MemberSeedRow>();

            var rows = new List<MemberSeedRow>();

            foreach (var line in lines.Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var parts = line.Split(',');

                if (parts.Length < 11)
                    throw new Exception($"Members.csv 欄位數不足：{line}");

                rows.Add(new MemberSeedRow
                {
                    MemberID = parts[0].Trim(),
                    DisplayName = parts[1].Trim(),
                    Gender = byte.Parse(parts[2].Trim()),
                    Birthday = ParseNullableDate(parts[3]),
                    CreatedAt = DateTime.Parse(parts[4].Trim(), CultureInfo.InvariantCulture),
                    Phone = ParseNullableString(parts[5]),
                    Email = parts[6].Trim(),
                    Password = parts[7].Trim(),
                    StatusID = byte.Parse(parts[8].Trim()),
                    IsCreator = ParseBool(parts[9]),
                    IsAdmin = ParseBool(parts[10]),
                    AdminLevel = parts.Length > 11 ? ParseNullableString(parts[11]) : null
                });
            }

            return rows;
        }

        private static string? ParseNullableString(string value)
        {
            var text = value?.Trim();
            return string.IsNullOrWhiteSpace(text) ? null : text;
        }

        private static DateTime? ParseNullableDate(string value)
        {
            var text = value?.Trim();
            if (string.IsNullOrWhiteSpace(text))
                return null;

            return DateTime.Parse(text, CultureInfo.InvariantCulture);
        }

        private static bool ParseBool(string value)
        {
            var text = value?.Trim();

            if (string.IsNullOrWhiteSpace(text))
                return false;

            return text switch
            {
                "1" => true,
                "0" => false,
                _ => bool.Parse(text)
            };
        }
    }
}
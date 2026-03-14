namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedInventoryHelper
    {
        public static int GetStockQty(string productId, string stockLevelType)
        {
            if (string.IsNullOrWhiteSpace(productId))
                throw new ArgumentException("ProductID 不可為空");

            var numericPart = ExtractNumericPart(productId);

            return stockLevelType.Trim().ToLowerInvariant() switch
            {
                "normal" => 10 + (numericPart % 71), // 10 ~ 80
                "low" => 1 + (numericPart % 5),      // 1 ~ 5
                "zero" => 0,
                _ => throw new Exception($"未知的 StockLevelType：{stockLevelType}")
            };
        }

        private static int ExtractNumericPart(string productId)
        {
            var digits = new string(productId.Where(char.IsDigit).ToArray());

            if (!int.TryParse(digits, out var value))
                throw new Exception($"無法從 ProductID 解析數字：{productId}");

            return value;
        }
    }
}
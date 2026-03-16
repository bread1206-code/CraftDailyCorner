namespace CraftDailyCorner.Seed.Demo.Helpers
{
    public static class DemoSeedPortfolioTimeHelper
    {
        public static DateTime GetPortfolioCreatedAt(int brandNo)
        {
            if (brandNo <= 0)
                throw new ArgumentOutOfRangeException(nameof(brandNo), "品牌序號必須大於 0");

            var baseDate = new DateTime(2026, 3, 1, 0, 0, 0);

            var dayOffset = (brandNo - 1) % 22;
            var hour = 9 + ((brandNo - 1) % 8);          // 9 ~ 16
            var minute = ((brandNo - 1) * 7) % 60;
            var second = ((brandNo - 1) * 11) % 60;

            return baseDate
                .AddDays(dayOffset)
                .AddHours(hour)
                .AddMinutes(minute)
                .AddSeconds(second);
        }
    }
}
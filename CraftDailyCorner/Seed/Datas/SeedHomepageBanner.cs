using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedHomepageBanner
    {
        private readonly CraftDailyCornerContext _context;

        public SeedHomepageBanner(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.HomepageBanners.Any()) // 避免重複 Seed
            {
                var homepageBanners = new List<HomepageBanner>
                {
                    new HomepageBanner
                    {
                        ImageUrl = imageGuids[0] + ".png",
                        Title = "手作職人市集",
                        Subtitle = "慢活 × 原創 × 溫度",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[1] + ".png",
                        Title = "手作職人市集",
                        Subtitle = "慢活 × 原創 × 溫度",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[2] + ".png",
                        Title = "新年快樂",
                        Subtitle = "日作市集祝福您",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    }
                };
                _context.HomepageBanners.AddRange(homepageBanners);
                _context.SaveChanges();
            }
        }
    }
}

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
                        Title = "日作市集",
                        Subtitle = "探索手工與在地溫暖",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[1] + ".png",
                        Title = "新年快樂",
                        Subtitle = "日作市集祝賀您",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[2] + ".png",
                        Title = "日作市集",
                        Subtitle = "發現手工美好，感受春光溫煦",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[3] + ".png",
                        Title = "日作市集",
                        Subtitle = "",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[4] + ".png",
                        Title = "日作市集",
                        Subtitle = "",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },new HomepageBanner
                    {
                        ImageUrl = imageGuids[5] + ".png",
                        Title = "日作市集",
                        Subtitle = "",
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

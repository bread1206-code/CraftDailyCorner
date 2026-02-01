using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPlatformAnnouncement
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPlatformAnnouncement(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.PlatformAnnouncements.Any()) // 避免重複 Seed
            {
                var platformAnnouncements = new List<PlatformAnnouncement>
                {
                    new PlatformAnnouncement
                    {
                        Title = "平台正式上線",
                        Content = "歡迎加入手作市集平台！",
                        StatusID = 2,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },
                    new PlatformAnnouncement
                    {
                        Title = "春節出貨公告",
                        Content = "春節期間出貨將延後 3–5 日。",
                        StatusID = 2,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    }
                };
                _context.PlatformAnnouncements.AddRange(platformAnnouncements);
                _context.SaveChanges();
            }
        }
    }
}

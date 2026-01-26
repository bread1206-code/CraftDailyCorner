using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorApplication
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorApplication(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.CreatorApplication.Any()) // 避免重複 Seed
            {
                var creatorApplications = new List<CreatorApplication>
                {
                    new CreatorApplication
                    {
                        DisplayName = "木匠大師",
                        Intro = "我是阿拓。",
                        PortfolioSampleUrl = imageGuids[0] + ".png",
                        StartDate = new DateTime(2020, 01, 01),
                        Status = (CreatorApplicationStatus)2,
                        AppliedAt = new DateTime(2025, 12, 01),
                        ReviewedAt = new DateTime(2025, 12, 03),
                        ReviewNote = "簡介太少",
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }, new CreatorApplication
                    {
                        DisplayName = "木匠大師",
                        Intro = "我是阿拓。這雙手除了與木材對話，別無長處。" +
                        "我專注於打磨帶有溫潤手感的木牌項鍊，也雕琢能盛裝回憶的榫接置物盒。" +
                        "每一道木紋都是時間的贈禮，我用鑿刀留住森林的氣息，" +
                        "只為將這份靜謐的陪伴，送到你的掌心。",
                        PortfolioSampleUrl = imageGuids[1] + ".png",
                        StartDate = new DateTime(2020, 01, 01),
                        Status = (CreatorApplicationStatus)1,
                        AppliedAt = new DateTime(2025, 12, 03),
                        ReviewedAt = new DateTime(2025, 12, 04),
                        ReviewNote = null,
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }, new CreatorApplication
                    {
                        DisplayName = "墨尋",
                        Intro = "我是墨尋，一生只在黑白之間修行。" +
                        "除了書寫紅紙黑字的春聯與氣勢磅礴的詩詞掛軸，我也將筆墨染上手工摺扇，捕捉流動的清風。" +
                        "我筆下的每一點一畫，不求驚世駭俗，只願在墨香散去前，為你這浮躁的世間留下一抹安定的神韻。",
                        PortfolioSampleUrl = imageGuids[2] + ".png",
                        StartDate = new DateTime(2020, 08, 01),
                        Status = (CreatorApplicationStatus)1,
                        AppliedAt = new DateTime(2025, 12, 01),
                        ReviewedAt = new DateTime(2025, 12, 03),
                        ReviewNote = null,
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }
                };
                _context.CreatorApplication.AddRange(creatorApplications);
                _context.SaveChanges();
            }
        }
    }
}

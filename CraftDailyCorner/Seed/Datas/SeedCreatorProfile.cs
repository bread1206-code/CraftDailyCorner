using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorProfile
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorProfile(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.CreatorProfiles.Any()) // 避免重複 Seed
            {
                var creatorProfiles = new List<CreatorProfile>
                {
                    new CreatorProfile
                    {
                        CreatorID = "C00001",
                        ImageUrl = imageGuids[0],
                        BrandName = "木匠大師",
                        BrandIntro = "我是阿拓。這雙手除了與木材對話，別無長處。" +
                        "我專注於打磨帶有溫潤手感的木牌項鍊，也雕琢能盛裝回憶的榫接置物盒。" +
                        "每一道木紋都是時間的贈禮，我用鑿刀留住森林的氣息，" +
                        "只為將這份靜謐的陪伴，送到你的掌心。",
                        StartDate = new DateTime(2020, 01, 01),
                        BankCode = " ",
                        BankAccount = " ",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000004",
                        UpdatedAt = DateTime.Now
                    }, new CreatorProfile
                    {
                        CreatorID = "C00002",
                        ImageUrl = imageGuids[1],
                        BrandName = "墨尋",
                        BrandIntro = "我是墨尋，一生只在黑白之間修行。" +
                        "除了書寫紅紙黑字的春聯與氣勢磅礴的詩詞掛軸，我也將筆墨染上手工摺扇，捕捉流動的清風。" +
                        "我筆下的每一點一畫，不求驚世駭俗，只願在墨香散去前，為你這浮躁的世間留下一抹安定的神韻。",
                        StartDate = new DateTime(2020, 08, 01),
                        BankCode = " ",
                        BankAccount = " ",
                        StatusID = 1,
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000005",
                        UpdatedAt = DateTime.Now
                    }
                };
                _context.CreatorProfiles.AddRange(creatorProfiles);
                _context.SaveChanges();
            }
        }
    }
}

using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedTag
    {
        private readonly CraftDailyCornerContext _context;

        public SeedTag(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Tag.Any()) // 避免重複 Seed
            {
                var tags = new List<Tag>
                {
                    new Tag { TagName = "手工" },
                    new Tag { TagName = "限量" },
                    new Tag { TagName = "原創" }
                };
                _context.Tag.AddRange(tags);
                _context.SaveChanges();
            }
        }
    }
}

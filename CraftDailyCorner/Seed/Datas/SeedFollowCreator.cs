using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedFollowCreator
    {
        private readonly CraftDailyCornerContext _context;

        public SeedFollowCreator(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.FollowCreator.Any()) // 避免重複 Seed
            {
                var followCreators = new List<FollowCreator>
                {
                    new FollowCreator
                    {
                        MemberID = "M0000002",
                        CreatorID = "C00001",
                        CreatedAt = DateTime.Now
                    }
                };
                _context.FollowCreator.AddRange(followCreators);
                _context.SaveChanges();
            }
        }
    }
}

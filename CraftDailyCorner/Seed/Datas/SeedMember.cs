using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedMember
    {
        private readonly CraftDailyCornerContext _context;

        public SeedMember(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(string[] imageGuids)
        {
            if (!_context.Members.Any()) // 避免重複 Seed
            {
                var members = new List<Member>
            {
                new Member
                {
                    MemberID = "M0000001",
                    ImageUrl = imageGuids[0] + ".png",
                    DisplayName = "一號會員",
                    Status = 0,
                    CreatedAt = DateTime.Now
                },
                new Member
                {
                    MemberID = "M0000002",
                    ImageUrl = imageGuids[1] + ".png",
                    DisplayName = "二號會員",
                    Status = 0,
                    CreatedAt = DateTime.Now
                },
                new Member
                {
                    MemberID = "M0000003",
                    ImageUrl = imageGuids[2] + ".png",
                    DisplayName = "三號會員",
                    Status = 0,
                    CreatedAt = DateTime.Now
                },
                new Member
                {
                    MemberID = "M0000004",
                    ImageUrl = imageGuids[3] + ".png",
                    DisplayName = "四號會員",
                    Status = 0,
                    CreatedAt = DateTime.Now
                },
                new Member
                {
                    MemberID = "M0000005",
                    ImageUrl = imageGuids[4] + ".png",
                    DisplayName = "五號會員",
                    Status = 0,
                    CreatedAt = DateTime.Now
                }
            };

                _context.Members.AddRange(members);
                _context.SaveChanges();
            }
        }
    }
}
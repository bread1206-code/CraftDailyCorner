using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPrivacy
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPrivacy(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Privacy.Any()) // 避免重複 Seed
            {
                var privacies = new List<Privacy>
                {
                    new Privacy
                    {
                        Email = "member01@member.com",
                        PasswordHash = "111",
                        Phone = "0912345671",
                        Birthday = new DateTime(2025, 12, 20),
                        Gender = 0,
                        MemberID = "M0000001"
                    }, new Privacy
                    {
                        Email = "member02@member.com",
                        PasswordHash = "222",
                        Phone = "0912345672",
                        Birthday = new DateTime(2025, 12, 21),
                        Gender = 0,
                        MemberID = "M0000002"
                    }, new Privacy
                    {
                        Email = "member03@member.com",
                        PasswordHash = "333",
                        Phone = "0912345673",
                        Birthday = new DateTime(2025, 12, 22),
                        Gender = 0,
                        MemberID = "M0000003"
                    }, new Privacy
                    {
                        Email = "member04@member.com",
                        PasswordHash = "444",
                        Phone = "0912345674",
                        Birthday = new DateTime(2025, 12, 23),
                        Gender = 0,
                        MemberID = "M0000004"
                    }, new Privacy
                    {
                        Email = "member05@member.com",
                        PasswordHash = "555",
                        Phone = "0912345675",
                        Birthday = new DateTime(2025, 12, 24),
                        Gender = 0,
                        MemberID = "M0000005"
                    }
                };
                _context.Privacy.AddRange(privacies);
                _context.SaveChanges();
            }
        }
    }
}

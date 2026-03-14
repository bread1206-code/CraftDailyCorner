using CraftDailyCorner.Models;
using Microsoft.AspNetCore.Identity;

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
            if (!_context.Privacies.Any()) // 避免重複 Seed
            {
                var hasher = new PasswordHasher<Privacy>(); // 新增 Hasher

                var privacies = new List<Privacy>
                {
                    new Privacy
                    {
                        Email = "member00@member.com",
                        Phone = "0912345000",
                        Birthday = new DateTime(2025, 12, 20),
                        Gender = 0,
                        MemberID = "M0000000"
                    }
                    //,new Privacy
                    //{
                    //    Email = "member01@member.com",
                    //    Phone = "0912345671",
                    //    Birthday = new DateTime(2025, 12, 20),
                    //    Gender = 0,
                    //    MemberID = "M0000001"
                    //}, new Privacy
                    //{
                    //    Email = "member02@member.com",
                    //    Phone = "0912345672",
                    //    Birthday = new DateTime(2025, 12, 21),
                    //    Gender = 0,
                    //    MemberID = "M0000002"
                    //}, new Privacy
                    //{
                    //    Email = "member03@member.com",
                    //    Phone = "0912345673",
                    //    Birthday = new DateTime(2025, 12, 22),
                    //    Gender = 0,
                    //    MemberID = "M0000003"
                    //}, new Privacy
                    //{
                    //    Email = "member04@member.com",
                    //    Phone = "0912345674",
                    //    Birthday = new DateTime(2025, 12, 23),
                    //    Gender = 0,
                    //    MemberID = "M0000004"
                    //}, new Privacy
                    //{
                    //    Email = "member05@member.com",
                    //    Phone = "0912345675",
                    //    Birthday = new DateTime(2025, 12, 24),
                    //    Gender = 0,
                    //    MemberID = "M0000005"
                    //}
                };
                // Hash 密碼
                privacies[0].PasswordHash = hasher.HashPassword(privacies[0], "000");
                //privacies[0].PasswordHash = hasher.HashPassword(privacies[0], "111");
                //privacies[1].PasswordHash = hasher.HashPassword(privacies[1], "222");
                //privacies[2].PasswordHash = hasher.HashPassword(privacies[2], "333");
                //privacies[3].PasswordHash = hasher.HashPassword(privacies[3], "444");
                //privacies[4].PasswordHash = hasher.HashPassword(privacies[4], "555");

                _context.Privacies.AddRange(privacies);
                _context.SaveChanges();
            }
        }
    }
}

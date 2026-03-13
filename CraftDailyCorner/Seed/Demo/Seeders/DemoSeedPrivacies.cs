using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Seed.Demo.Context;
using Microsoft.AspNetCore.Identity;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedPrivacies
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedPrivacies(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            if (_context.Privacies.Any())
                return;

            var hasher = new PasswordHasher<Privacy>();

            var privacies = seedContext.Members.Select(row =>
            {
                var privacy = new Privacy
                {
                    MemberID = row.MemberID,
                    Email = row.Email,
                    Phone = row.Phone,
                    Birthday = row.Birthday,
                    Gender = ConvertGender(row.Gender)
                };

                privacy.PasswordHash = hasher.HashPassword(privacy, row.Password);

                return privacy;
            }).ToList();

            _context.Privacies.AddRange(privacies);
            _context.SaveChanges();
        }

        private static PrivacyGender ConvertGender(byte gender)
        {
            return gender switch
            {
                0 => (PrivacyGender)0,
                1 => (PrivacyGender)1,
                2 => (PrivacyGender)2,
                _ => throw new Exception($"不支援的 Gender 值：{gender}")
            };
        }
    }
}
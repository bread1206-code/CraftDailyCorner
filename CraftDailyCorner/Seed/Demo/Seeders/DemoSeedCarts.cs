using CraftDailyCorner.Models;
using CraftDailyCorner.Seed.Demo.Context;

namespace CraftDailyCorner.Seed.Demo.Seeders
{
    public class DemoSeedCarts
    {
        private readonly CraftDailyCornerContext _context;

        public DemoSeedCarts(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run(DemoSeedContext seedContext)
        {
            if (seedContext == null)
                throw new ArgumentNullException(nameof(seedContext));

            if (seedContext.Members == null || !seedContext.Members.Any())
                throw new Exception("DemoSeedContext.Members 沒有資料");

            var existingMemberIds = _context.Carts
                .Select(x => x.MemberID)
                .ToHashSet();

            var carts = seedContext.Members
                .Where(row => !existingMemberIds.Contains(row.MemberID))
                .Select(row => new Cart
                {
                    MemberID = row.MemberID,
                    CreatedAt = row.CreatedAt
                })
                .ToList();

            if (carts.Any())
            {
                _context.Carts.AddRange(carts);
                _context.SaveChanges();
            }
        }
    }
}
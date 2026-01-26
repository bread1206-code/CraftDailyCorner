using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCart
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCart(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Cart.Any()) // 避免重複 Seed
            {
                var carts = new List<Cart>
                {
                    new Cart
                    {
                        MemberID = "M0000002",
                        UpdatedAt = DateTime.Now
                    }
                };
                _context.Cart.AddRange(carts);
                _context.SaveChanges();
            }
        }
    }
}

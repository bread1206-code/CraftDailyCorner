using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCartItem
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCartItem(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.CartItems.Any()) // 避免重複 Seed
            {
                var cartItems = new List<CartItem>
                {
                    new CartItem
                    {
                        CartID = 1,
                        ProductID = "P000000001",
                        Quantity = 1
                    }
                };
                _context.CartItems.AddRange(cartItems);
                _context.SaveChanges();
            }
        }
    }
}

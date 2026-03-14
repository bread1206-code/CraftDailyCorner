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
            if (!_context.Carts.Any()) // 避免重複 Seed
            {
                var carts = new List<Cart>
                {
                    new Cart
                    {
                        MemberID = "M0000000",
                        CreatedAt = DateTime.Now
                    }
                    //,new Cart
                    //{
                    //    MemberID = "M0000001",
                    //    CreatedAt = DateTime.Now
                    //},
                    //new Cart
                    //{
                    //    MemberID = "M0000002",
                    //    CreatedAt = DateTime.Now
                    //},
                    //new Cart
                    //{
                    //    MemberID = "M0000003",
                    //    CreatedAt = DateTime.Now
                    //},
                    //new Cart
                    //{
                    //    MemberID = "M0000004",
                    //    CreatedAt = DateTime.Now
                    //},
                    //new Cart
                    //{
                    //    MemberID = "M0000005",
                    //    CreatedAt = DateTime.Now
                    //}
                };
                _context.Carts.AddRange(carts);
                _context.SaveChanges();
            }
        }
    }
}

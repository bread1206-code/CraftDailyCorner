using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedFavoriteProduct
    {
        private readonly CraftDailyCornerContext _context;

        public SeedFavoriteProduct(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.FavoriteProducts.Any()) // 避免重複 Seed
            {
                var favoriteProducts = new List<FavoriteProduct>
                {
                    new FavoriteProduct
                    {
                        MemberID = "M0000002",
                        ProductID = "P000000001",
                        CreatedAt = DateTime.Now
                    }
                };
                _context.FavoriteProducts.AddRange(favoriteProducts);
                _context.SaveChanges();
            }
        }
    }
}

using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProductReview
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProductReview(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.ProductReview.Any()) // 避免重複 Seed
            {
                var productReviews = new List<ProductReview>
                {
                    new ProductReview
                    {
                        Rating = 5,
                        Comment = "質感非常好，會再回購",
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        ProductID = "P000000001"
                    }
                };
                _context.ProductReview.AddRange(productReviews);
                _context.SaveChanges();
            }
        }
    }
}

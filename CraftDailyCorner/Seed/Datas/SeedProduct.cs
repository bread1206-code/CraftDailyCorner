using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProduct
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProduct(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Products.Any()) // 避免重複 Seed
            {
                var products = new List<Product>
                {
                    new Product
                        {
                            ProductID = "P000000001",
                            ProductName = "木牌項鍊",
                            Description = "手工雕刻的溫潤木牌項鍊",
                            Price = 1200,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000002",
                            ProductName = "書法摺扇",
                            Description = "以行書書寫的手工摺扇",
                            Price = 1800,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00002"
                        }, new Product
                        {
                            ProductID = "P000000003",
                            ProductName = "原木書架",
                            Description = "手工打造的原木書架，保留自然木紋紋理，兼具美觀與實用。",
                            Price = 3500,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000004",
                            ProductName = "榫接茶几",
                            Description = "採用傳統榫接工法製作的手工茶几，穩固耐用，木紋自然清晰。",
                            Price = 4800,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000005",
                            ProductName = "行書字帖",
                            Description = "由書法家手寫的行書練習字帖，包含經典篇章與練習指南。",
                            Price = 8000,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00002"
                        }
                };
                _context.Products.AddRange(products);
                _context.SaveChanges();
            }
        }
    }
}

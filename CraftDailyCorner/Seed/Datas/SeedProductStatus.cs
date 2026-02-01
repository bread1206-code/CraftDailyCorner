using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProductStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProductStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.ProductStatuses.Any()) return;

            _context.ProductStatuses.AddRange(
                new ProductStatus
                {
                    StatusID = 1,
                    StatusCode = "Draft",
                    StatusName = "草稿",
                    Description = "尚未上架",
                    IsActive = true
                },
                new ProductStatus
                {
                    StatusID = 2,
                    StatusCode = "OnSale",
                    StatusName = "上架中",
                    Description = "商品可販售",
                    IsActive = true
                },
                new ProductStatus
                {
                    StatusID = 3,
                    StatusCode = "OffSale",
                    StatusName = "下架",
                    Description = "商品已下架",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

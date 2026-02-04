using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedProductImageStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedProductImageStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.ProductImageStatuses.Any()) return;

            _context.ProductImageStatuses.AddRange(
                new ProductImageStatus
                {
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "圖片正常顯示",
                    IsActive = true
                },
                new ProductImageStatus
                {
                    StatusID = 2,
                    StatusCode = "Hidden",
                    StatusName = "隱藏",
                    Description = "圖片被隱藏",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

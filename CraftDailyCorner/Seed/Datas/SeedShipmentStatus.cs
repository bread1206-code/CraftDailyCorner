using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedShipmentStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedShipmentStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.ShipmentStatuses.Any()) return;

            _context.ShipmentStatuses.AddRange(
                new ShipmentStatus
                {
                    StatusID = 1,
                    StatusCode = "Preparing",
                    StatusName = "備貨中",
                    Description = "準備出貨",
                    IsActive = true
                },
                new ShipmentStatus
                {
                    StatusID = 2,
                    StatusCode = "Shipped",
                    StatusName = "已出貨",
                    Description = "商品已寄出",
                    IsActive = true
                },
                new ShipmentStatus
                {
                    StatusID = 3,
                    StatusCode = "Delivered",
                    StatusName = "已送達",
                    Description = "商品已送達",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedShipment
    {
        private readonly CraftDailyCornerContext _context;

        public SeedShipment(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Shipments.Any()) // 避免重複 Seed
            {
                var shipments = new List<Shipment>
                {
                    new Shipment
                    {
                        TrackingNo = "EC123456789TW",
                        StatusID = 1,
                        OrderID = "202601010001"
                    }
                };
                _context.Shipments.AddRange(shipments);
                _context.SaveChanges();
            }
        }
    }
}

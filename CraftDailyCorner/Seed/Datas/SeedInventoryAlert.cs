using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedInventoryAlert
    {
        private readonly CraftDailyCornerContext _context;

        public SeedInventoryAlert(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.InventoryAlert.Any()) // 避免重複 Seed
            {
                var inventoryAlerts = new List<InventoryAlert>
                {
                    new InventoryAlert
                    {
                        TriggeredAt = DateTime.Now,
                        Status = (InventoryAlertStatus)1,
                        InventoryID = 1
                    }
                };
                _context.InventoryAlert.AddRange(inventoryAlerts);
                _context.SaveChanges();
            }
        }
    }
}

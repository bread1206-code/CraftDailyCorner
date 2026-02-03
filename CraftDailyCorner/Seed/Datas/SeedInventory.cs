using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedInventory
    {
        private readonly CraftDailyCornerContext _context;

        public SeedInventory(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Inventories.Any()) // 避免重複 Seed
            {
                var inventories = new List<Inventory>
                {
                    new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 5,
                        ProductID = "P000000001"
                    },new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 5,
                        ProductID = "P000000002"
                    },new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 5,
                        ProductID = "P000000003"
                    },new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 5,
                        ProductID = "P000000004"
                    },new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 5,
                        ProductID = "P000000005"
                    }
                };
                _context.Inventories.AddRange(inventories);
                _context.SaveChanges();
            }
        }
    }
}

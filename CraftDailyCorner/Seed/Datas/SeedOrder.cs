using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedOrder
    {
        private readonly CraftDailyCornerContext _context;

        public SeedOrder(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Order.Any()) // 避免重複 Seed
            {
                var orders = new List<Order>
                {
                    new Order
                    {
                        OrderID = "202601010001",
                        ReceiverName = "王小明",
                        ReceiverPhone = "0912345678",
                        ShippingAddress = "台北市中正區",
                        CreatedAt = DateTime.Now,
                        Status = 1,
                        TotalAmount = 1200,
                        MemberID = "M0000002"
                    }
                };
                _context.Order.AddRange(orders);
                _context.SaveChanges();
            }
        }
    }
}

using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedOrderDetail
    {
        private readonly CraftDailyCornerContext _context;

        public SeedOrderDetail(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.OrderDetail.Any()) // 避免重複 Seed
            {
                var orderDetails = new List<OrderDetail>
                {
                    new OrderDetail
                    {
                        OrderID = "202601010001",
                        ProductID = "P000000001",
                        ProductNameSnapshot = "木牌項鍊",
                        PriceSnapshot = 1200,
                        Quantity = 1
                    }
                };
                _context.OrderDetail.AddRange(orderDetails);
                _context.SaveChanges();
            }
        }
    }
}

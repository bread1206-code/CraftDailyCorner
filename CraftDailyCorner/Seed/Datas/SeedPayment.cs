using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPayment
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPayment(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run()
        {
            if (!_context.Payments.Any()) // 避免重複 Seed
            {
                var payments = new List<Payment>
                {
                    new Payment
                    {
                        MethodID = 1,
                        Amount = 1200,
                        StatusID = 1,
                        GatewayTradeNo = "TEST123456",
                        AttemptNo = 1,
                        PaidAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        OrderID = "202601010001"
                    }
                };
                _context.Payments.AddRange(payments);
                _context.SaveChanges();
            }
        }
    }
}

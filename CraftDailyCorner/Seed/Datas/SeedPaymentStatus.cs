using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPaymentStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPaymentStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PaymentStatuses.Any()) return;

            _context.PaymentStatuses.AddRange(
                new PaymentStatus
                {
                    StatusID = 1,
                    StatusCode = "Pending",
                    StatusName = "待付款",
                    Description = "尚未付款",
                    IsActive = true
                },
                new PaymentStatus
                {
                    StatusID = 2,
                    StatusCode = "Success",
                    StatusName = "成功",
                    Description = "付款成功",
                    IsActive = false
                },
                new PaymentStatus
                {
                    StatusID = 3,
                    StatusCode = "Failed",
                    StatusName = "失敗",
                    Description = "付款失敗",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

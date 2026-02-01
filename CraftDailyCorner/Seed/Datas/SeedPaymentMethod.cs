using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPaymentMethod
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPaymentMethod(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.PaymentMethods.Any()) return;

            _context.PaymentMethods.AddRange(
                new PaymentMethod
                {
                    MethodID = 1,
                    MethodCode = "CreditCard",
                    MethodName = "信用卡"
                },
                new PaymentMethod
                {
                    MethodID = 2,
                    MethodCode = "LinePay",
                    MethodName = "Line Pay"
                },
                new PaymentMethod
                {
                    MethodID = 3,
                    MethodCode = "BankTransfer",
                    MethodName = "銀行轉帳"
                }
            );

            _context.SaveChanges();
        }
    }

}

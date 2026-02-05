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
                    MethodName = "信用卡",
                    Description = "使用信用卡付款，支持Visa、MasterCard、American Express等主要信用卡品牌。",
                    IsActive = true
                },
                new PaymentMethod
                {
                    MethodID = 2,
                    MethodCode = "LinePay",
                    MethodName = "Line Pay",
                    Description = "使用Line Pay付款，透過Line帳號進行快速、安全的支付。",
                    IsActive = true
                },
                new PaymentMethod
                {
                    MethodID = 3,
                    MethodCode = "BankTransfer",
                    MethodName = "銀行轉帳",
                    Description = "使用銀行轉帳付款，請按照提供的銀行帳戶資訊進行轉帳。",
                    IsActive = true
                }
            );

            _context.SaveChanges();
        }
    }

}

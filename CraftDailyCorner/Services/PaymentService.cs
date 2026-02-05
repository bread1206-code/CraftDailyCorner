using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{


    public class PaymentService : IPaymentService
    {
        private readonly CraftDailyCornerContext _context;

        public PaymentService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 付款頁初始化
        public VMPaymentCreate PreparePayment(string orderId)
        {
            var order = _context.Orders
                .Include(o => o.OrderDetails)
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
                return null;

            return new VMPaymentCreate
            {
                OrderID = order.OrderID,
                OrderAmount = order.TotalAmount,
                OrderCreatedAt = order.CreatedAt,
                PaymentMethods = _context.PaymentMethods
                    .Where(m => m.IsActive)
                    .Select(m => new VMPaymentMethod
                    {
                        MethodID = m.MethodID,
                        MethodName = m.MethodName
                    })
                    .ToList()
            };
        }
        public VMPaymentResult CreateMockPayment(VMPaymentSubmit vm)
        {
            using var tx = _context.Database.BeginTransaction();

            try
            {
                // 取得訂單
                var order = _context.Orders
                    .FirstOrDefault(o => o.OrderID == vm.OrderID);

                if (order == null)
                    throw new Exception("訂單不存在");

                // 計算第幾次付款
                var attemptNo = _context.Payments
                    .Count(p => p.OrderID == vm.OrderID) + 1;

                // 取得狀態
                var paidStatusId = _context.PaymentStatuses
                    .Where(s => s.StatusCode == "Success")
                    .Select(s => s.StatusID)
                    .First();

                if (paidStatusId == 0)
                    throw new Exception("PaymentStatus: PAID 未設定");

                var failedStatusId = _context.PaymentStatuses
                    .Where(s => s.StatusCode == "Failed")
                    .Select(s => s.StatusID)
                    .First();

                var pendingOrderStatusId = _context.OrderStatuses
                    .Where(s => s.StatusCode == "Pending")
                    .Select(s => s.StatusID)
                    .First();

                var paidOrderStatusId = _context.OrderStatuses
                    .Where(s => s.StatusCode == "Paid")
                    .Select(s => s.StatusID)
                    .First();

                var isSuccess = vm.SimulateSuccess;

                var payment = new Payment
                {
                    OrderID = order.OrderID,
                    Amount = order.TotalAmount,
                    MethodID = vm.MethodID,
                    AttemptNo = (byte)attemptNo,
                    StatusID = isSuccess ? paidStatusId : failedStatusId,
                    GatewayTradeNo = $"MOCK-{Guid.NewGuid():N}",
                    CreatedAt = DateTime.Now,
                    PaidAt = isSuccess ? DateTime.Now : null
                };

                _context.Payments.Add(payment);

                // 同步訂單狀態
                order.StatusID = isSuccess
                    ? paidOrderStatusId
                    : pendingOrderStatusId;

                _context.SaveChanges();
                tx.Commit();

                return new VMPaymentResult
                {
                    OrderID = order.OrderID,
                    Amount = payment.Amount,
                    PaymentStatusID = payment.StatusID,
                    PaymentStatusName = isSuccess ? "付款成功" : "付款失敗",
                    PaidAt = payment.PaidAt,
                    Message = isSuccess? "付款完成，感謝您的訂購！" : "付款失敗，請重新嘗試"
                };
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }

        public List<VMPaymentRecord> GetPaymentsByOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return new List<VMPaymentRecord>();

            var payments = _context.Payments
                .Where(p => p.OrderID == orderId)
                .Include(p => p.PaymentStatus)
                .Include(p => p.PaymentMethod)
                .OrderBy(p => p.AttemptNo)
                .Select(p => new VMPaymentRecord
                {
                    PaymentID = p.PaymentID,
                    Amount = p.Amount,
                    AttemptNo = p.AttemptNo,

                    StatusID = p.StatusID,
                    StatusName = p.PaymentStatus.StatusName,

                    MethodName = p.PaymentMethod.MethodName,

                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToList();

            return payments;
        }
    }
}

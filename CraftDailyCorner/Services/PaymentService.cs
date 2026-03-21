using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Payment;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly INotificationService _notificationService;

        public PaymentService(
            CraftDailyCornerContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }


        // 付款頁初始化

        public VMPaymentCreate? PreparePayment(string orderId)
        {
            var order = _context.Orders
                .AsNoTracking()
                .FirstOrDefault(o => o.OrderID == orderId);

            if (order == null)
                return null;

            var paidOrderStatusId = _context.OrderStatuses
                .Where(s => s.StatusCode == "Paid")
                .Select(s => s.StatusID)
                .FirstOrDefault();

            if (paidOrderStatusId == 0)
                throw new Exception("OrderStatus: Paid 未設定");

            return new VMPaymentCreate
            {
                OrderID = order.OrderID,
                OrderAmount = order.TotalAmount,
                OrderCreatedAt = order.CreatedAt,

                // ⭐ 是否已付款（給 Controller / View 用）
                IsPaid = order.StatusID == paidOrderStatusId,

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


        // 建立付款（Mock）

        public VMPaymentResult CreateMockPayment(VMPaymentSubmit vm)
        {
            using var tx = _context.Database.BeginTransaction();

            try
            {
                var now = DateTime.Now;

                // 取得訂單（for update）
                var order = _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.CreatorProfile)
                    .FirstOrDefault(o => o.OrderID == vm.OrderID);

                if (order == null)
                    throw new Exception("訂單不存在");

                // 取得訂單狀態
                var paidOrderStatusId = _context.OrderStatuses
                    .Where(s => s.StatusCode == "Paid")
                    .Select(s => s.StatusID)
                    .FirstOrDefault();

                var pendingOrderStatusId = _context.OrderStatuses
                    .Where(s => s.StatusCode == "Pending")
                    .Select(s => s.StatusID)
                    .FirstOrDefault();

                if (paidOrderStatusId == 0 || pendingOrderStatusId == 0)
                    throw new Exception("OrderStatus 設定不完整");

                // 已付款訂單禁止再次付款（最重要）
                if (order.StatusID == paidOrderStatusId)
                    throw new Exception("此訂單已完成付款，請勿重複付款");

                // 付款狀態
                var paidPaymentStatusId = _context.PaymentStatuses
                    .Where(s => s.StatusCode == "Success")
                    .Select(s => s.StatusID)
                    .FirstOrDefault();

                var failedPaymentStatusId = _context.PaymentStatuses
                    .Where(s => s.StatusCode == "Failed")
                    .Select(s => s.StatusID)
                    .FirstOrDefault();

                if (paidPaymentStatusId == 0 || failedPaymentStatusId == 0)
                    throw new Exception("PaymentStatus 設定不完整");

                // 計算付款次數
                var attemptNo = _context.Payments
                    .Count(p => p.OrderID == vm.OrderID) + 1;

                var isSuccess = vm.SimulateSuccess;

                // 建立付款紀錄
                var payment = new Payment
                {
                    OrderID = order.OrderID,
                    Amount = order.TotalAmount,
                    MethodID = vm.MethodID,
                    AttemptNo = (byte)attemptNo,
                    StatusID = isSuccess ? paidPaymentStatusId : failedPaymentStatusId,
                    GatewayTradeNo = $"MOCK-{Guid.NewGuid():N}",//模擬第三方交易編號
                    CreatedAt = now,
                    PaidAt = isSuccess ? now : null
                };

                _context.Payments.Add(payment);

                // 同步訂單狀態
                order.StatusID = isSuccess
                    ? paidOrderStatusId
                    : pendingOrderStatusId;

                _context.SaveChanges();

                // ===== 付款成功通知 =====
                if (isSuccess)
                {
                    // 會員：付款完成通知
                    _context.NotificationEvents.Add(new NotificationEvent
                    {
                        MemberID = order.MemberID,
                        NotificationType = NotificationType.OrderPaid,
                        Title = "付款完成通知",
                        Content = $"訂單 {order.OrderID} 已完成付款。",
                        LinkUrl = $"/Orders/Detail?orderId={order.OrderID}",
                        IsRead = false,
                        RelatedEntityType = "Order",
                        RelatedEntityId = order.OrderID,
                        CreatedAt = now
                    });

                    // 創作者：付款完成通知（準備出貨）
                    var creatorMemberIds = order.OrderDetails
                        .Where(x => x.Product?.CreatorProfile != null)
                        .Select(x => x.Product.CreatorProfile!.MemberID)
                        .Distinct()
                        .ToList();

                    foreach (var creatorMemberId in creatorMemberIds)
                    {
                        _context.NotificationEvents.Add(new NotificationEvent
                        {
                            MemberID = creatorMemberId,
                            NotificationType = NotificationType.OrderPaid,
                            Title = "新訂單通知",
                            Content = $"訂單 {order.OrderID} 已完成付款，請準備出貨。",
                            LinkUrl = $"/CreatorOrders/Detail?id={order.OrderID}",
                            IsRead = false,
                            RelatedEntityType = "Order",
                            RelatedEntityId = order.OrderID,
                            CreatedAt = now
                        });
                    }

                    _context.SaveChanges();
                }

                tx.Commit();

                return new VMPaymentResult
                {
                    OrderID = order.OrderID,
                    Amount = payment.Amount,
                    PaymentStatusID = payment.StatusID,
                    PaymentStatusName = isSuccess ? "付款成功" : "付款失敗",
                    PaidAt = payment.PaidAt,
                    Message = isSuccess
                        ? "付款完成，感謝您的訂購！"
                        : "付款失敗，請重新嘗試"
                };
            }
            catch
            {
                tx.Rollback();
                throw;
            }
        }


        // 查詢訂單付款紀錄
        public List<VMPaymentIndexItem> GetMyPayments(string memberId)
        {
            if (string.IsNullOrEmpty(memberId))
                return new List<VMPaymentIndexItem>();

            return _context.Payments
                .Include(p => p.Order)
                .Include(p => p.PaymentMethod)
                .Include(p => p.PaymentStatus)
                .Where(p => p.Order.MemberID == memberId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMPaymentIndexItem
                {
                    OrderID = p.OrderID,
                    Amount = p.Amount,
                    MethodName = p.PaymentMethod.MethodName,
                    StatusName = p.PaymentStatus.StatusName,
                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToList();
        }

        public List<VMPaymentRecord> GetPaymentsByOrder(string orderId)
        {
            if (string.IsNullOrEmpty(orderId))
                return new List<VMPaymentRecord>();

            return _context.Payments
                .AsNoTracking()
                .Include(p => p.PaymentMethod)
                .Include(p => p.PaymentStatus)
                .Where(p => p.OrderID == orderId)
                .OrderByDescending(p => p.CreatedAt)
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
        }
    }
}
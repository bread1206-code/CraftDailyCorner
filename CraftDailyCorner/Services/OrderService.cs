using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Member;
using CraftDailyCorner.ViewModels.Order;
using CraftDailyCorner.ViewModels.Payment;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CraftDailyCorner.Services
{
    public class OrderService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly CartService _cartService;

        public OrderService(
            CraftDailyCornerContext context,
            CartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // 建立訂單
        public VMCreateOrderResult CreateOrder(string memberId, VMCreateOrderRequest request, string creatorId)
        {
            // 1. 開啟交易，確保「扣庫存」與「建訂單」是一體操作
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 2. 直接查詢購物車實體 (Entity) 而非 ViewModel
                // 這樣我們可以直接操作 inventory 物件並讓 EF 追蹤修改
                var cartItems = _context.CartItems
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.Inventory) // 務必包含庫存表
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.CreatorProfile)
                    .Where(ci => ci.Cart.MemberID == memberId && ci.Product.CreatorID == creatorId)
                    .ToList();

                if (!cartItems.Any())
                {
                    return new VMCreateOrderResult { Success = false, Message = "購物車內無該創作者的商品" };
                }

                // 3. 建立 Order 主檔
                var orderId = GetNewOrderID();
                var totalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

                var order = new Order
                {
                    OrderID = orderId,
                    MemberID = memberId,
                    CreatedAt = DateTime.Now,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ShippingAddress = request.ReceiverAddress,
                    TotalAmount = totalAmount,
                    StatusID = 1 // 待付款
                };
                _context.Orders.Add(order);

                // 4. 逐項檢查庫存、扣除庫存、並建立訂單明細
                foreach (var item in cartItems)
                {
                    var product = item.Product;
                    var inventory = product.Inventory;

                    // --- 庫存校驗 ---
                    if (inventory == null || inventory.StockQty < item.Quantity)
                    {
                        // 若庫存不足，直接拋出例外觸發 Rollback
                        throw new Exception($"商品「{product.ProductName}」庫存不足（僅剩 {inventory?.StockQty ?? 0} 件）");
                    }

                    // --- 扣除庫存 ---
                    inventory.StockQty -= item.Quantity;
                    inventory.UpdatedAt = DateTime.Now;

                    // --- 建立訂單明細 ---
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductID,
                        ProductNameSnapshot = product.ProductName,
                        PriceSnapshot = product.Price,
                        CostSnapshot = product.CostPrice,
                        Quantity = item.Quantity
                    };
                    _context.OrderDetails.Add(orderDetail);
                }

                // 5. 清空該創作者在購物車中的商品
                _context.CartItems.RemoveRange(cartItems);

                // 6. 一次性儲存：這會包含 Order 的 Insert、OrderDetail 的 Insert、Inventory 的 Update、CartItem 的 Delete
                _context.SaveChanges();

                // 7. 提交交易
                transaction.Commit();

                return new VMCreateOrderResult
                {
                    Success = true,
                    Message = "訂單建立成功",
                    OrderID = order.OrderID
                };
            }
            catch (Exception ex)
            {
                // 發生錯誤（如庫存不足）時自動回滾，保護資料
                transaction.Rollback();
                return new VMCreateOrderResult { Success = false, Message = ex.Message };
            }
        }

        // 我的訂單列表
        public List<VMMyOrder> GetMyOrders(string memberId, string? statusCode)
        {
            var query = _context.Orders
                    .AsNoTracking()
                    .Include(o => o.OrderStatus)
                    .Where(o => o.MemberID == memberId);
            query = statusCode switch
            {
                "padding" =>
                    query.Where(o => o.OrderStatus.StatusID == 1),

                "processing" =>
                    query.Where(o => o.OrderStatus.StatusID == 2 || o.OrderStatus.StatusID == 3 || o.OrderStatus.StatusID == 4),

                _ => query // All
            };

            return query
                   .OrderByDescending(o => o.CreatedAt)
                   .Select(o => new VMMyOrder
                   {
                       OrderID = o.OrderID,
                       CreatedAt = o.CreatedAt,
                       TotalAmount = (int)Math.Floor(o.TotalAmount),
                       StatusText = o.OrderStatus.StatusName,
                       UpdatedAt = o.UpdatedAt
                   })
                   .ToList();
        }

        // 我的訂單詳細內容
        public VMMyOrderDetail? GetOrderDetail(string orderId, string memberId)
        {
            var order = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderStatus)
                .Include(o => o.Shipment)
                    .ThenInclude(s => s.ShipmentStatus)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.CreatorProfile)
                .FirstOrDefault(o =>
                    o.OrderID == orderId &&
                    o.MemberID == memberId);

            if (order == null)
                return null;

            // ⭐ 付款紀錄
            var payments = _context.Payments
                .Where(p => p.OrderID == orderId)
                .OrderBy(p => p.CreatedAt)
                .Select(p => new VMOrderPaymentItem
                {
                    Amount = p.Amount,
                    MethodName = p.PaymentMethod.MethodName,
                    StatusName = p.PaymentStatus.StatusName,
                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToList();

            return new VMMyOrderDetail
            {
                OrderID = order.OrderID,
                CreatedAt = order.CreatedAt,
                TotalAmount = (int)Math.Floor(order.TotalAmount),
                StatusText = order.OrderStatus.StatusName,
                StatusID = order.OrderStatus.StatusID,

                ReceiverName = order.ReceiverName,
                ReceiverPhone = order.ReceiverPhone,
                ShippingAddress = order.ShippingAddress,

                Items = order.OrderDetails!
                    .Select(od => new VMMyOrderItem
                    {
                        ProductID = od.ProductID,
                        ProductName = od.ProductNameSnapshot,   // 快照
                        Price = (int)Math.Floor(od.PriceSnapshot),
                        Quantity = od.Quantity,
                        CreatorName = od.Product.CreatorProfile!.DisplayName
                    })
                    .ToList(),
                OrderPayments = new VMOrderPaymentList
                {
                    OrderID = order.OrderID,
                    Payments = payments
                },

                ShipmentStatusID = order.Shipment?.StatusID,
                TrackingNo = order.Shipment?.TrackingNo,
                ShippedAt = order.Shipment?.ShippedAt,
                DeliveredAt = order.Shipment?.DeliveredAt
            };
        }

        // 產生訂單編號
        private string GetNewOrderID()
        {
            var outputParam = new SqlParameter
            {
                ParameterName = "@NewOrderID",
                SqlDbType = SqlDbType.NChar,
                Size = 12,
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC getCreatedOrderID @NewOrderID OUTPUT",
                outputParam
            );

            return outputParam.Value!.ToString()!;
        }
        // 取消訂單
        // 檔案：Services/OrderService.cs

        public (bool Success, string Message) CancelOrder(string orderId, string memberId)
        {
            // 使用交易確保「狀態變更」與「庫存回補」同時成功
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // 1. 取得訂單，並包含明細與庫存資料
                var order = _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Inventory)
                    .FirstOrDefault(o => o.OrderID == orderId && o.MemberID == memberId);

                if (order == null) return (false, "找不到該訂單");

                // 2. 嚴格限制：只有「待付款 (StatusID = 1)」才能取消
                if (order.StatusID != 1)
                {
                    return (false, "目前訂單狀態不可取消（僅限待付款訂單）");
                }

                // 3. 庫存回補邏輯
                if (order.OrderDetails != null)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        if (detail.Product.Inventory != null)
                        {
                            // 將當初扣除的數量加回去
                            detail.Product.Inventory.StockQty += detail.Quantity;
                            detail.Product.Inventory.UpdatedAt = DateTime.Now;
                        }
                    }
                }

                // 4. 變更訂單狀態為「取消 (StatusID = 6)」
                order.StatusID = 6;
                order.UpdatedAt = DateTime.Now;
                _context.SaveChanges();
                transaction.Commit();

                return (true, "訂單已成功取消，庫存已釋出");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return (false, "系統錯誤，無法取消訂單");
            }
        }
        //模擬物流
        public (bool Success, string Message) ConfirmPickup(string orderId, string memberId)
        {
            using var tx = _context.Database.BeginTransaction();
            try
            {
                var order = _context.Orders
                    .Include(o => o.Shipment)
                    .FirstOrDefault(o => o.OrderID == orderId && o.MemberID == memberId);

                if (order == null) return (false, "找不到該訂單");

                // 必須配送中
                if (order.StatusID != 4)
                    return (false, "目前訂單狀態不可取貨（僅限配送中）");

                if (order.Shipment == null)
                    return (false, "找不到物流資訊，無法取貨");

                // Shipment 更新：已送達(3) + DeliveredAt
                order.Shipment.StatusID = 3;
                order.Shipment.DeliveredAt = DateTime.Now;

                // 同步更新 Order 狀態 與 UpdatedAt
                order.StatusID = 5;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                tx.Commit();
                return (true, "已更新為『已送達』，請點擊完成訂單");
            }
            catch
            {
                tx.Rollback();
                return (false, "系統錯誤，取貨失敗");
            }
        }

        public (bool Success, string Message) CompleteOrder(string orderId, string memberId)
        {
            using var tx = _context.Database.BeginTransaction();
            try
            {
                var order = _context.Orders
                    .Include(o => o.Shipment)
                    .FirstOrDefault(o => o.OrderID == orderId && o.MemberID == memberId);

                if (order == null) return (false, "找不到該訂單");

                // 已取消不可完成
                if (order.StatusID == 6) return (false, "此訂單已取消");

                // 必須已送達（Shipment=3）
                if (order.Shipment == null || order.Shipment.StatusID != 3)
                    return (false, "尚未送達，無法完成訂單");

                // Order 更新：完成(5)
                order.StatusID = 5;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                tx.Commit();
                return (true, "訂單已完成，感謝您的購買！");
            }
            catch
            {
                tx.Rollback();
                return (false, "系統錯誤，完成訂單失敗");
            }
        }
    }
}

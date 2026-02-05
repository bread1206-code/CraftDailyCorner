using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels;
using CraftDailyCorner.ViewModels.Front;
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
        public VMCreateOrderResult CreateOrder(
            string memberId,
            VMCreateOrderRequest request)
        {
            // 1️ 取得購物車快照
            var cartItems = _cartService.GetCartItemsForCheckout(memberId);

            if (!cartItems.Any())
            {
                return new VMCreateOrderResult
                {
                    Success = false,
                    Message = "購物車是空的，無法建立訂單"
                };
            }

            // 2️ 計算總金額
            var totalAmount = cartItems.Sum(i =>
                i.Product.Price * i.Quantity);

            // 3️ 建立 Order 主檔
            var orderId = GetNewOrderID();
            var order = new Order
            {
                OrderID = orderId,
                MemberID = memberId,
                CreatedAt = DateTime.Now,
                ReceiverName = request.ReceiverName,
                ReceiverPhone = request.ReceiverPhone,
                ShippingAddress = request.ReceiverAddress,
                TotalAmount = totalAmount,
                StatusID = 1 // 未付款
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // 先存，取得 OrderID

            // 4️ 建立 OrderItems（商品快照）
            foreach (var item in cartItems)
            {
                var orderItem = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.Product.ProductId,
                    ProductNameSnapshot = item.Product.ProductName,
                    PriceSnapshot = item.Product.Price,
                    Quantity = item.Quantity
                };

                _context.OrderDetails.Add(orderItem);
            }

            _context.SaveChanges();

            // 5️ 清空購物車
            _cartService.ClearCart(memberId);

            return new VMCreateOrderResult
            {
                Success = true,
                Message = "訂單建立成功",
                OrderID = order.OrderID
            };
        }

        // 我的訂單列表
        public List<VMMyOrder> GetMyOrders(string memberId)
        {
            return _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderStatus)
                .Where(o => o.MemberID == memberId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new VMMyOrder
                {
                    OrderID = o.OrderID,
                    CreatedAt = o.CreatedAt,
                    TotalAmount = (int)Math.Floor(o.TotalAmount),
                    StatusText = o.OrderStatus.StatusName
                })
                .ToList();
        }
        // 我的訂單詳細內容
        public VMMyOrderDetail? GetOrderDetail(string orderId, string memberId)
        {
            var order = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderStatus)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.CreatorProfile)
                .FirstOrDefault(o =>
                    o.OrderID == orderId &&
                    o.MemberID == memberId);

            if (order == null)
                return null;

            return new VMMyOrderDetail
            {
                OrderID = order.OrderID,
                CreatedAt = order.CreatedAt,
                TotalAmount = (int)Math.Floor(order.TotalAmount),
                StatusText = order.OrderStatus.StatusName,

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
                    .ToList()
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
    }
}

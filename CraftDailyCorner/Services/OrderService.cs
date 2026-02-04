using CraftDailyCorner.Models;
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
                "EXEC getOrderID @NewOrderID OUTPUT",
                outputParam
            );

            return outputParam.Value!.ToString()!;
        }
    }
}

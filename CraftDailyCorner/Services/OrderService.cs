//using CraftDailyCorner.Models;
//using CraftDailyCorner.ViewModels.Front;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Data.SqlClient;
//using Microsoft.EntityFrameworkCore;

//namespace CraftDailyCorner.Services
//{
//    public class OrderService
//    {
//        private readonly CraftDailyCornerContext _context;

//        public OrderService(CraftDailyCornerContext context)
//        {
//            _context = context;
//        }

//        public async Task<string> CreateOrderAsync(string memberId, List<VMCartItem> cartItems, string receiverName,string receiverPhone,string receiverAddress)
//        {
//            using var tx = await _context.Database.BeginTransactionAsync();
//            try
//            {
//                // 1. 呼叫 SP 取得 OrderID
//                var newOrderIdParam = new SqlParameter
//                {
//                    ParameterName = "@NewOrderID",
//                    SqlDbType = System.Data.SqlDbType.Char,
//                    Size = 12,
//                    Direction = System.Data.ParameterDirection.Output
//                };

//                await _context.Database.ExecuteSqlRawAsync(
//                    "EXEC getCreateOrder @NewOrderID OUTPUT",
//                    newOrderIdParam
//                );

//                string newOrderId = newOrderIdParam.Value!.ToString()!;


//                // 2. 建立 Order 主檔
//                var order = new Order
//                {
//                    OrderID = newOrderId,
//                    MemberID = memberId,
//                    ReceiverName = receiverName,
//                    ReceiverPhone = receiverPhone,
//                    ShippingAddress = receiverAddress,
//                    CreatedAt = DateTime.Now,
//                    TotalAmount = cartItems.Sum(i => (int)Math.Floor(i.Price * i.Quantity)),
//                    StatusID = 1
//                };
//                _context.Orders.Add(order);

//                // 3. 建立 OrderDetail
//                foreach (var item in cartItems)
//                {
//                    var orderItem = new OrderDetail
//                    {
//                        OrderID = newOrderId,
//                        ProductID = item.ProductId,
//                        ProductNameSnapshot= item.ProductName,
//                        PriceSnapshot = item.Price,
//                        Quantity = item.Quantity
//                    };
//                    _context.OrderDetails.Add(orderItem);
//                }

//                // 4. 清空購物車
//                _context.CartItems.RemoveRange(
//                    _context.CartItems.Where(c => c.Cart.MemberID == memberId)
//                );

//                await _context.SaveChangesAsync();
//                await tx.CommitAsync();

//                return newOrderId;
//            }
//            catch
//            {
//                await tx.RollbackAsync();
//                throw;
//            }
//        }
//    }

//}

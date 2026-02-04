using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class OrderService
    {
        private readonly CraftDailyCornerContext _context;

        public OrderService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<string> CreateOrderAsync(
            string memberId,
            List<VMCartItem> cartItems,
            string receiverName,
            string receiverPhone,
            string receiverAddress)
        {
            if (cartItems == null || !cartItems.Any())
                throw new InvalidOperationException("購物車是空的");

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 1️⃣ 建立 Order
                var order = new Order
                {
                    OrderID = GenerateOrderId(),
                    MemberID = memberId,
                    ReceiverName = receiverName,
                    ReceiverPhone = receiverPhone,
                    ShippingAddress = receiverAddress,
                    StatusID = 1,//待付款
                    CreatedAt = DateTime.Now
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // 2️⃣ 逐項驗證庫存 + 建立 OrderItem
                foreach (var item in cartItems)
                {
                    var inventory = await _context.Inventories
                        .FirstOrDefaultAsync(i => i.ProductID == item.ProductID);

                    if (inventory == null)
                        throw new InvalidOperationException("找不到商品庫存");

                    if (inventory.StockQty < item.Quantity)
                        throw new InvalidOperationException(
                            $"商品 {item.ProductName} 庫存不足");

                    // 建立 OrderDetail
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        ProductNameSnapshot = item.ProductName,
                        PriceSnapshot = item.Price
                    });

                    // 扣庫存
                    inventory.StockQty -= item.Quantity;
                }

                // 3️⃣ 清空購物車（DB）
                var cart = await _context.Carts
                    .FirstOrDefaultAsync(c => c.MemberID == memberId);

                if (cart != null)
                {
                    var cartItemsDb = _context.CartItems
                        .Where(ci => ci.CartID == cart.CartID);

                    _context.CartItems.RemoveRange(cartItemsDb);
                }

                // 4️⃣ 一次 SaveChanges
                await _context.SaveChangesAsync();

                // 5️⃣ Commit
                await tx.CommitAsync();

                return order.OrderID;
            }
            catch
            {
                await tx.RollbackAsync();
                throw; // 交給 Controller 處理錯誤訊息
            }
        }

        private string GenerateOrderId()
        {
            return $"OD{DateTime.Now:yyyyMMddHHmmssfff}";
        }
    }
}

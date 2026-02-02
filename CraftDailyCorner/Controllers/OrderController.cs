using CraftDailyCorner.Models;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace CraftDailyCorner.Controllers
{
    public class OrderController : Controller
    {
        private readonly CraftDailyCornerContext _context;
        private readonly CartService _cartService;
        private readonly PriceService _priceService;

        public OrderController(CraftDailyCornerContext context, CartService cartService, PriceService priceService)
        {
            _context = context;
            _cartService = cartService;
            _priceService = priceService;
        }

        // 結帳入口

        public IActionResult GoCheckout()
        {
            return View();
        }

        // 結帳確認頁（顯示訂單）
        // GET: /Order/Checkout
        [Authorize]
        public IActionResult Checkout()
        {
            var cart = _cartService.GetSessionCart();

            // 購物車空的，不能結帳
            if (cart == null || !cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }
            var vm = new VMCheckout
            {
                Items = cart,
                TotalAmount = _priceService.CalculateTotal(cart)
            };

            return View(vm);
        }

        // 3送出訂單（真正寫 DB）
        // POST: /Order/CheckoutConfirm
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CheckoutConfirm()
        {
            var cart = _cartService.GetSessionCart();

            if (!cart.Any())
            {
                return RedirectToAction("Index", "Home");
            }

            // ⚠ 一定要後端重算
            var totalAmount = _priceService.CalculateTotal(cart);
            var memberId = GetCurrentMemberId().ToString();

            // 建立 Order
            var order = new Order
            {
                MemberID = memberId,
                TotalAmount = totalAmount,
                StatusID = 1, // 待付款
                CreatedAt = DateTime.Now
            };

            _context.Orders.Add(order);
            _context.SaveChanges(); // 先存，拿 OrderId

            // 建立 OrderItems
            foreach (var item in cart)
            {
                var orderItem = new OrderDetail
                {
                    OrderID = order.OrderID,
                    ProductID = item.ProductID,
                    ProductNameSnapshot = item.ProductName,
                    PriceSnapshot = item.Price,
                    Quantity = item.Quantity,
                };

                _context.OrderDetails.Add(orderItem);
            }

            _context.SaveChanges();

            // 清空購物車（Session + DB）
            _cartService.ClearSessionCart();
            ClearCartFromDb(memberId);

            return RedirectToAction("Success", new { id = order.OrderID });
        }

        // 4️結帳完成頁
        // GET: /Order/Success/5
        [Authorize]
        public IActionResult Success(string id)
        {
            var order = _context.Orders
                .FirstOrDefault(o => o.OrderID == id);

            if (order == null)
                return NotFound();

            return View(order);
        }

        //  Private Helpers
        // 從 Claims 取得 MemberId
        private int GetCurrentMemberId()
        {
            return int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );
        }

        // 結帳完成後清除 DB 購物車
        private void ClearCartFromDb(string memberId)
        {
            var items = _context.CartItems
                .Include(c => c.Cart)
                .Where(c => c.Cart.MemberID == memberId);

            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}

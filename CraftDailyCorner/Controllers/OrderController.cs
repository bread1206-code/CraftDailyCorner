//using CraftDailyCorner.Models;
//using CraftDailyCorner.Services;
//using CraftDailyCorner.ViewModels.Front;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System.Security.Claims;


//namespace CraftDailyCorner.Controllers
//{
//    public class OrderController : Controller
//    {
//        private readonly CraftDailyCornerContext _context;
//        private readonly CartService _cartService;
//        private readonly PriceService _priceService;
//        private readonly OrderService _orderService;

//        public OrderController(CraftDailyCornerContext context, CartService cartService, PriceService priceService, OrderService orderService)
//        {
//            _context = context;
//            _cartService = cartService;
//            _priceService = priceService;
//            _orderService = orderService;
//        }

//        // 結帳確認頁（顯示訂單）
//        // GET: /Order/Checkout
//        [Authorize]
//        public IActionResult Checkout()
//        {
//            string memberId = GetCurrentMemberId();

//            var cart = _cartService.GetCartItemsForCheckout(memberId);

//            if (cart == null || !cart.Any())
//            {
//                return RedirectToAction("Index", "Home");
//            }

//            var vm = new VMCheckout
//            {
//                Items = cart,
//                TotalAmount = _priceService.CalculateTotal(cart)
//            };

//            return View(vm);
//        }

//        // 3送出訂單（真正寫 DB）
//        // POST: /Order/CheckoutConfirm
//        [HttpPost]
//        [Authorize]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> CheckoutConfirm(VMCheckout vm)
//        {
//            if (!ModelState.IsValid)
//            {
//                return View("Checkout", vm); // 回到確認頁顯示錯誤
//            }

//            string memberId = GetCurrentMemberId();

//            var cartItems = _cartService.GetCartItemsForCheckout(memberId);

//            string orderId = await _orderService.CreateOrderAsync(
//                memberId,
//                cartItems,
//                vm.ReceiverName,
//                vm.ReceiverPhone,
//                vm.ReceiverAddress
//            );

//            return RedirectToAction("Success", new { id = orderId });
//        }


//        // 4️結帳完成頁
//        // GET: /Order/Success/5
//        [Authorize]
//        public IActionResult Success(string id)
//        {
//            var order = _context.Orders
//                .FirstOrDefault(o => o.OrderID == id);

//            if (order == null)
//                return NotFound();

//            return View(order);
//        }

//        //  Private Helpers
//        // 從 Claims 取得 MemberId
//        private string GetCurrentMemberId()
//        {
//            return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
//        }

//    }
//}

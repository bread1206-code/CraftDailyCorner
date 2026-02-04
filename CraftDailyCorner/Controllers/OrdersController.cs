using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public OrdersController(
            CartService cartService,
            OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        // GET: /Orders/Checkout
        public IActionResult Checkout()
        {
            var memberId = GetMemberId();

            var items = _cartService.GetCartItemsForCheckout(memberId);

            if (!items.Any())
            {
                TempData["Error"] = "購物車是空的";
                return RedirectToAction("Index", "Products");
            }

            var vm = new VMCheckout
            {
                Items = items,
                TotalAmount = items.Sum(i => i.Product.Price * i.Quantity)
            };

            return View(vm);
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMCreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                // 表單驗證失敗 → 回 Checkout
                var memberId = GetMemberId();
                var items = _cartService.GetCartItemsForCheckout(memberId);

                var vm = new VMCheckout
                {
                    Items = items,
                    TotalAmount = items.Sum(i => i.Product.Price * i.Quantity),
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ReceiverAddress = request.ReceiverAddress
                };

                return View("Checkout", vm);
            }

            var result = _orderService.CreateOrder(GetMemberId(), request);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Checkout));
            }

            // 成功 → 導向訂單完成頁
            return RedirectToAction(nameof(Complete), new { orderId = result.OrderID });
        }

        // GET: /Orders/Complete/{orderId}
        public IActionResult Complete(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return NotFound();

            // 目前先顯示訂單編號即可
            // 之後可以擴充查詢 OrderDetail
            ViewBag.OrderID = orderId;

            return View();
        }

        // Private

        private string GetMemberId()
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(memberId))
                throw new UnauthorizedAccessException("找不到會員識別資訊");

            return memberId;
        }
    }
}

using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Order;
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
        //我的訂單列表
        public IActionResult Index(string statusCode)
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var orders = _orderService.GetMyOrders(memberId!, statusCode);

            return View(orders);
        }

        
        
        //訂單詳細內容
        public IActionResult Detail(string orderId)
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = _orderService.GetOrderDetail(orderId, memberId!);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: /Orders/Checkout
        public IActionResult Checkout(string creatorId)
        {
            var memberId = GetMemberId();
            if (string.IsNullOrEmpty(creatorId)) return RedirectToAction("Index", "Cart");

            // 取得該創作者的商品清單
            var allItems = _cartService.GetCartItemsForCheckout(memberId);
            var filteredItems = allItems.Where(i => i.Product.CreatorId == creatorId).ToList();

            if (!filteredItems.Any()) return RedirectToAction("Index", "Products");

            var vm = new VMCheckout
            {
                Items = filteredItems,
                TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                CreatorId = creatorId // 傳給 View
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
                var memberId = GetMemberId();
                // 驗證失敗重新載入時，也要帶回該創作者的商品
                var allItems = _cartService.GetCartItemsForCheckout(memberId);
                var filteredItems = allItems.Where(i => i.Product.CreatorId == request.CreatorId).ToList();

                var vm = new VMCheckout
                {
                    Items = filteredItems,
                    TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                    CreatorId = request.CreatorId,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ReceiverAddress = request.ReceiverAddress
                };
                return View("Checkout", vm);
            }

            // ⭐ 這裡傳入第三個參數 request.CreatorId
            var result = _orderService.CreateOrder(GetMemberId(), request, request.CreatorId);

            if (!result.Success)
            {
                TempData["Error"] = result.Message;
                // 如果失敗，導回結帳頁並帶上 creatorId
                return RedirectToAction(nameof(Checkout), new { creatorId = request.CreatorId });
            }

            return RedirectToAction(nameof(Complete), new { orderId = result.OrderID });
        }

        // GET: /Orders/Complete/{orderId}
        public IActionResult Complete(string orderId)
        {
            if (string.IsNullOrWhiteSpace(orderId))
                return NotFound();

            var vm = new VMOrderDetail
            {
                OrderID = orderId
            };

            return View(vm);
        }
        //取消訂單
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(string orderId)
        {
            var memberId = GetMemberId();
            var result = _orderService.CancelOrder(orderId, memberId);

            if (result.Success)
            {
                TempData["Success"] = result.Message;
                // 取消成功後導向明細頁，明細頁會因為狀態改變而隱藏付款/取消按鈕
                return RedirectToAction(nameof(Detail), new { orderId = orderId });
            }
            else
            {
                TempData["Error"] = result.Message;
                return RedirectToAction(nameof(Detail), new { orderId = orderId });
            }
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

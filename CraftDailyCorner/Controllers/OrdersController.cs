using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Order;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        // 我的訂單列表
        public IActionResult Index(string statusCode)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var orders = _orderService.GetMyOrders(memberId, statusCode);

            return View(orders);
        }

        // 訂單詳細內容
        public IActionResult Detail(string orderId)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var order = _orderService.GetOrderDetail(orderId, memberId);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: /Orders/Checkout
        public IActionResult Checkout(string creatorId, string? selectedProductIds)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            if (string.IsNullOrEmpty(creatorId))
                return RedirectToAction("Index", "Cart");

            var selectedProductIdList = (selectedProductIds ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            if (!selectedProductIdList.Any())
            {
                TempData["OrdersError"] = "請先勾選要結帳的商品";
                return RedirectToAction("Index", "Products");
            }

            var filteredItems = _cartService.GetCartItemsForCheckout(memberId, creatorId, selectedProductIdList);

            if (!filteredItems.Any())
                return RedirectToAction("Index", "Products");

            TempData.Remove("Error");
            ViewBag.HideCartButton = true;

            var vm = new VMCheckout
            {
                Items = filteredItems,
                TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                CreatorId = creatorId,
                SelectedProductIds = filteredItems
                    .Select(i => i.Product.ProductId)
                    .Distinct()
                    .ToList()
            };

            return View(vm);
        }

        // POST: /Orders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(VMCreateOrderRequest request)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            if (request.SelectedProductIds == null || !request.SelectedProductIds.Any())
            {
                ModelState.AddModelError(string.Empty, "請先勾選要結帳的商品");
            }

            if (!ModelState.IsValid)
            {
                var filteredItems = _cartService.GetCartItemsForCheckout(
                    memberId,
                    request.CreatorId,
                    request.SelectedProductIds ?? new List<string>());

                ViewBag.HideCartButton = true;

                var vm = new VMCheckout
                {
                    Items = filteredItems,
                    TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                    CreatorId = request.CreatorId,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ReceiverAddress = request.ReceiverAddress,
                    SelectedProductIds = request.SelectedProductIds ?? new List<string>()
                };

                return View("Checkout", vm);
            }

            var result = _orderService.CreateOrder(memberId, request);

            if (!result.Success)
            {
                var filteredItems = _cartService.GetCartItemsForCheckout(
                    memberId,
                    request.CreatorId,
                    request.SelectedProductIds ?? new List<string>());

                var vm = new VMCheckout
                {
                    Items = filteredItems,
                    TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                    CreatorId = request.CreatorId,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ReceiverAddress = request.ReceiverAddress,
                    SelectedProductIds = request.SelectedProductIds ?? new List<string>()
                };

                TempData["OrdersError"] = result.Message;
                return View("Checkout", vm);
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

        // 取消訂單
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(string orderId)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var result = _orderService.CancelOrder(orderId, memberId);

            if (result.Success)
            {
                TempData["OrdersSuccess"] = result.Message;
                return RedirectToAction(nameof(Detail), new { orderId });
            }

            TempData["OrdersError"] = result.Message;
            return RedirectToAction(nameof(Detail), new { orderId });
        }

        // 模擬會員取貨：配送中(訂單狀態=4)才允許
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPickup(string orderId)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrWhiteSpace(memberId))
                return Unauthorized();

            var result = _orderService.ConfirmPickup(orderId, memberId);

            TempData[result.Success ? "OrdersSuccess" : "OrdersError"] = result.Message;
            return RedirectToAction(nameof(Detail), new { orderId });
        }
    }
}
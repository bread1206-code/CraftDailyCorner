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
        //我的訂單列表
        public IActionResult Index(string statusCode)
        {
            var memberId = User.GetMemberId();
            var orders = _orderService.GetMyOrders(memberId!, statusCode);

            return View(orders);
        }

        //訂單詳細內容
        public IActionResult Detail(string orderId)
        {
            var memberId = User.GetMemberId();

            var order = _orderService.GetOrderDetail(orderId, memberId!);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // GET: /Orders/Checkout
        // GET: /Orders/Checkout
        public IActionResult Checkout(string creatorId, string? selectedProductIds)
        {
            var memberId = User.GetMemberId();
            if (string.IsNullOrEmpty(creatorId)) return RedirectToAction("Index", "Cart");

            // 新增：解析前端傳來的勾選商品清單
            var selectedProductIdList = (selectedProductIds ?? string.Empty)
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            if (!selectedProductIdList.Any())
            {
                TempData["Error"] = "請先勾選要結帳的商品";
                return RedirectToAction("Index", "Products");
            }

            // 取得該創作者且本次有勾選的商品清單
            var filteredItems = _cartService.GetCartItemsForCheckout(memberId, creatorId, selectedProductIdList);

            if (!filteredItems.Any()) return RedirectToAction("Index", "Products");

            //成功進入結帳頁時，清除前一次殘留的錯誤訊息
            TempData.Remove("Error");

            //標記目前是結帳流程頁，Navbar 可依此隱藏購物車按鈕
            ViewBag.HideCartButton = true;

            var vm = new VMCheckout
            {
                Items = filteredItems,
                TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                CreatorId = creatorId, // 傳給 View

                //將勾選商品清單帶進結帳頁，供 POST 回傳
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

            //先補強勾選商品驗證，避免只靠欄位 Required
            if (request.SelectedProductIds == null || !request.SelectedProductIds.Any())
            {
                ModelState.AddModelError(string.Empty, "請先勾選要結帳的商品");
            }

            if (!ModelState.IsValid)
            {
                //驗證失敗重新載入時，也要帶回本次勾選的商品
                var filteredItems = _cartService.GetCartItemsForCheckout(
                    memberId,
                    request.CreatorId,
                    request.SelectedProductIds ?? new List<string>());

                //這裡雖然網址是 /Orders/Create，但仍要隱藏購物車按鈕
                ViewBag.HideCartButton = true;

                var vm = new VMCheckout
                {
                    Items = filteredItems,
                    TotalAmount = filteredItems.Sum(i => i.Product.Price * i.Quantity),
                    CreatorId = request.CreatorId,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ReceiverAddress = request.ReceiverAddress,

                    //重新帶回本次勾選商品，避免畫面驗證失敗後遺失
                    SelectedProductIds = request.SelectedProductIds ?? new List<string>()
                };
                return View("Checkout", vm);
            }

            var result = _orderService.CreateOrder(memberId, request);

            if (!result.Success)
            {
                // 新增：失敗時直接回到結帳頁，保留原本輸入與勾選商品
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

                TempData["Error"] = result.Message;
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
        //取消訂單
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(string orderId)
        {
            var memberId = User.GetMemberId();
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
        // 模擬會員取貨：配送中(訂單狀態=4)才允許
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmPickup(string orderId)
        {
            var memberId = User.GetMemberId();
            var result = _orderService.ConfirmPickup(orderId, memberId);

            TempData[result.Success ? "Success" : "Error"] = result.Message;
            return RedirectToAction(nameof(Detail), new { orderId });
        }

    }
}
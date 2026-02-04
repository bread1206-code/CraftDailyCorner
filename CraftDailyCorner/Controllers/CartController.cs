using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    [Authorize] //必須登入
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }


         // 加入購物車

        [HttpPost]
        public IActionResult AddItem(string productId, int quantity)
        {
            var memberId = GetMemberId();

            var result = _cartService.AddItem(memberId, productId, quantity);
            return Json(result);
        }


         // 更新商品數量

        [HttpPost]
        public IActionResult UpdateQuantity(string productId, int quantity)
        {
            var memberId = GetMemberId();

            var result = _cartService.UpdateQuantity(memberId, productId, quantity);
            return Json(result);
        }

        // 移除商品
        [HttpPost]
        public IActionResult RemoveItem(string productId)
        {
            var memberId = GetMemberId();

            var result = _cartService.RemoveItem(memberId, productId);
            return Json(result);
        }

        // 取得購物車清單（Modal / 頁面）
        [HttpGet]
        public IActionResult GetCartItems()
        {
            var memberId = GetMemberId();

            var items = _cartService.GetCartItems(memberId);
            return PartialView("_CartItemsPartial", items);
        }

        // 取得購物車數量（Navbar Badge）
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var memberId = GetMemberId();

            var count = _cartService.GetCartCount(memberId);
            return Json(count);
        }

        // Private Helper
        private string GetMemberId()
        {
            var memberId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(memberId))
            {
                throw new UnauthorizedAccessException("找不到會員識別資訊");
            }
            return memberId;

        }
    }
}

using CraftDailyCorner.Services;
using CraftDailyCorner.ViewModels.Front.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CraftDailyCorner.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        // 加入購物車（唯一入口）
        [HttpPost]
        public IActionResult Add([FromBody] AddCartDTO req)
        {
            if (req == null)
                return BadRequest();

            bool isAuth = User.Identity?.IsAuthenticated ?? false;
            string? memberId = isAuth
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var result = _cartService.AddToCart(
                req.ProductId,
                req.Qty,
                isAuth,
                memberId
            );

            return Json(result);
        }


        // 移除商品
        [HttpPost]
        public IActionResult Remove([FromBody] AddCartDTO req)
        {
            if (req == null)
                return BadRequest();

            bool isAuth = User.Identity?.IsAuthenticated ?? false;
            string? memberId = isAuth
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var result = _cartService.RemoveFromCart(
                req.ProductId,
                isAuth,
                memberId
            );

            return Json(result);
        }

        // 重新取得 Cart Modal
        [HttpGet]
        public IActionResult GetCartModal()
        {
            return ViewComponent("VCCartModal");
        }

        // 取得購物車數量（Badge）
        [HttpGet]
        public IActionResult GetCartCount()
        {
            bool isAuth = User.Identity?.IsAuthenticated ?? false;
            string? memberId = isAuth
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var count = _cartService.GetCartCount(isAuth, memberId);
            return Json(new { count });
        }
    }
}

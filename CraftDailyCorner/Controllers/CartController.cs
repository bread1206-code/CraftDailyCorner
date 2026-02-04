//using CraftDailyCorner.Services;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace CraftDailyCorner.Controllers
//{
//    [Authorize]
//    public class CartController : Controller
//    {
//        private readonly CartService _cartService;

//        public CartController(CartService cartService)
//        {
//            _cartService = cartService;
//        }

//        [HttpPost]
//        public IActionResult AddItem(string productId, int quantity)
//        {
//            var memberId = User.FindFirst("MemberId")!.Value;

//            _cartService.AddItem(memberId, productId, quantity);
//            return Ok();
//        }

//        [HttpPost]
//        public IActionResult RemoveItem(string productId)
//        {
//            var memberId = User.FindFirst("MemberId")!.Value;

//            _cartService.RemoveItem(memberId, productId);
//            return Ok();
//        }

//        [HttpGet]
//        public IActionResult GetCartItem()
//        {
//            var memberId = User.FindFirst("MemberId")!.Value;

//            var data = _cartService.GetCartItem(memberId);
//            return PartialView("_CartModal", data);
//        }

//        [HttpGet]
//        public IActionResult GetCartCount()
//        {
//            var memberId = User.FindFirst("MemberId")!.Value;

//            var count = _cartService.GetCartCount(memberId);
//            return Json(count);
//        }
//    }
//}

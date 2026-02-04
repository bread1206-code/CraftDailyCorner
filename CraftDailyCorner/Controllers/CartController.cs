using CraftDailyCorner.Extensions;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Controllers
{
    public class CartController : Controller
    {
        private readonly CraftDailyCornerContext _context;

        public CartController(CraftDailyCornerContext context)
        {
            _context = context;
        }


        // 加入購物車
        [HttpPost]
        public IActionResult AddToCart([FromBody] AddCartDTO req)
        {
            // 1. 找商品
            var product = _context.Products
                .Where(p => p.ProductID == req.ProductId && p.StatusID == 2)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,
                    ImageUrl = p.ProductImages
                        .Where(img => img.StatusID == 1)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (product == null)
                return Json(new { success = false });

            //已登入，存 DB
            if (User.Identity!.IsAuthenticated)
            {
                string memberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                // 找 Cart
                var cart = _context.Carts.FirstOrDefault(c => c.MemberID == memberId);
                if (cart == null)
                {
                    cart = new Cart
                    {
                        MemberID = memberId,
                        CreatedAt = DateTime.Now
                    };
                    _context.Carts.Add(cart);
                    _context.SaveChanges();
                }

                // 找 CartItem
                var cartItem = _context.CartItems
                    .FirstOrDefault(c => c.CartID == cart.CartID && c.ProductID == product.ProductID);

                if (cartItem != null)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    _context.CartItems.Add(new CartItem
                    {
                        CartID = cart.CartID,
                        ProductID = product.ProductID,
                        Quantity = 1,
                    });
                }

                _context.SaveChanges();

                return Json(new { success = true });
            }
            // 未登入，存 Session
            else
            {
                var cart = HttpContext.Session
                    .GetObjectFromJson<List<VMCartItem>>("CART")
                    ?? new List<VMCartItem>();

                var item = cart.FirstOrDefault(c => c.ProductID == product.ProductID);

                if (item != null)
                    item.Quantity++;
                else
                    cart.Add(new VMCartItem
                    {
                        ProductID = product.ProductID,
                        ProductName = product.ProductName,
                        Price = product.Price,
                        Quantity = 1,
                        ImageUrl = product.ImageUrl
                    });

                HttpContext.Session.SetObjectAsJson("CART", cart);

                return Json(new { success = true });
            }
        }


        // 移除商品
        [HttpPost]
        public IActionResult RemoveFromModal([FromBody] AddCartDTO req)
        {
            var cart = HttpContext.Session
        .GetObjectFromJson<List<VMCartItem>>("CART")
        ?? new List<VMCartItem>();

            var item = cart.FirstOrDefault(c => c.ProductID == req.ProductId);

            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("CART", cart);
            }

            return Json(new { success = true });
        }


        public IActionResult StartCheckout()
        {
            if (!User.Identity!.IsAuthenticated)
            {
                return RedirectToAction(
                    "Login",
                    "Account",
                    new { returnUrl = Url.Action("GoCheckout", "Order") }
                );
            }

            return RedirectToAction("GoCheckout");
        }

        [HttpGet]
        public IActionResult GetCartModal()
        {
            return ViewComponent("VCCartModal");
        }
        [HttpGet]
        public IActionResult GetCartCount()
        {
            int count = 0;

            if (User.Identity!.IsAuthenticated)
            {
                string memberId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value;

                count = _context.CartItems
                    .Where(c => c.Cart.MemberID == memberId)
                    .Sum(c => c.Quantity);
            }
            else
            {
                var cart = HttpContext.Session
                    .GetObjectFromJson<List<VMCartItem>>("CART")
                    ?? new List<VMCartItem>();

                count = cart.Sum(c => c.Quantity);
            }

            return Json(new { count });
        }


    }
}

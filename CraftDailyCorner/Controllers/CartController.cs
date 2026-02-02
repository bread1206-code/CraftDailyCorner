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
        public IActionResult AddToCart(string productID)
        {
            // 1. 找商品
            var product = _context.Products
                .Where(p => p.ProductID == productID && p.StatusID == 2)
                .Select(p => new
                {
                    p.ProductID,
                    p.ProductName,
                    p.Price,

                    ImageUrl = p.ProductImages
                        .Where(img => img.StatusID==1)
                        .Select(img => img.ImageUrl)
                        .FirstOrDefault()
                })
                .FirstOrDefault();

            if (product == null)
                return NotFound();

            // 2. 從 Session 取購物車
            var cart = HttpContext.Session
                .GetObjectFromJson<List<VMCartItem>>("CART")
                ?? new List<VMCartItem>();

            // 3. 判斷是否已存在
            var item = cart.FirstOrDefault(c => c.ProductID == productID);

            if (item != null)
            {
                item.Quantity++;
            }
            else
            {
                cart.Add(new VMCartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = 1,
                    ImageUrl = product.ImageUrl
                });
            }

            // 4. 存回 Session
            HttpContext.Session.SetObjectAsJson("CART", cart);

            // 5. 回到原頁（或購物車頁）
            return RedirectToAction("Detail", "Products", new { id = productID });
        }

        // 移除商品
        [HttpPost]
        public IActionResult RemoveFromModal(string productID)
        {
            var cart = HttpContext.Session
                .GetObjectFromJson<List<VMCartItem>>("CART")
                ?? new List<VMCartItem>();

            var item = cart.FirstOrDefault(c => c.ProductID == productID);
            if (item != null)
            {
                cart.Remove(item);
                HttpContext.Session.SetObjectAsJson("CART", cart);
            }

            // 刪完後回到原頁（Modal 會重新 render）
            return Redirect(Request.Headers["Referer"].ToString());
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


    }
}

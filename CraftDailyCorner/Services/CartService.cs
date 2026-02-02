using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace CraftDailyCorner.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _http;
        private readonly CraftDailyCornerContext _context;
        //Session 取購物車
        private const string CART_KEY = "CART";
        public CartService(
            IHttpContextAccessor http,
            CraftDailyCornerContext context)
        {
            _http = http;
            _context = context;
        }

        private Cart GetOrCreateCart(string memberId)
        {
            memberId = memberId.Trim();

            var cart = _context.Carts
                .FirstOrDefault(c => c.MemberID == memberId);

            if (cart != null)
                return cart;

            cart = new Cart
            {
                MemberID = memberId,
                CreatedAt = DateTime.Now
            };

            _context.Carts.Add(cart);
            _context.SaveChanges();

            return cart;
        }


        

        public List<VMCartItem> GetSessionCart()
        {
            var session = _http.HttpContext!.Session;
            var json = session.GetString(CART_KEY);

            return json == null
                ? new List<VMCartItem>()
                : JsonSerializer.Deserialize<List<VMCartItem>>(json)!;
        }
        public bool HasSessionCart()
        {
            return GetSessionCart().Any();
        }

        //Session 存購物車
        public void SetSessionCart(List<VMCartItem> cart)
            {
                var json = JsonSerializer.Serialize(cart);
                _http.HttpContext!.Session.SetString(CART_KEY, json);
            }

        //清空 Session（登出 / 結帳後）
        public void ClearSessionCart()
        {
            _http.HttpContext!.Session.Remove(CART_KEY);
        }

        //登入成功後「同步購物車」
        public void SyncCartAfterLogin(string memberId)
        {
            var sessionCart = GetSessionCart();
            if (!sessionCart.Any())
                return;

            var cart = GetOrCreateCart(memberId);

            foreach (var item in sessionCart)
            {
                var dbItem = _context.CartItems
                    .FirstOrDefault(ci =>
                        ci.CartID == cart.CartID &&
                        ci.ProductID == item.ProductID);

                if (dbItem == null)
                {
                    _context.CartItems.Add(new CartItem
                    {
                        CartID = cart.CartID,
                        ProductID = item.ProductID,
                        Quantity = item.Quantity,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    dbItem.Quantity += item.Quantity;
                    dbItem.UpdatedAt = DateTime.Now;
                }
            }

            _context.SaveChanges();
            ClearSessionCart();
        }

        //登入後從 DB 還原購物車
        public void LoadCartFromDb(string memberId)
        {
            var cart = GetOrCreateCart(memberId);

            var items = _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.CartID == cart.CartID)
                .Select(ci => new VMCartItem
                {
                    ProductID = ci.ProductID,
                    ProductName = ci.Product.ProductName,      
                    Price = ci.Product.Price,       
                    ImageUrl = ci.Product.ProductImages.FirstOrDefault().ImageUrl,    
                    Quantity = ci.Quantity
                })
                .ToList();

            SetSessionCart(items);
        }

    }
}

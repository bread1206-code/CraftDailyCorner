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

        public CartService(
            IHttpContextAccessor http,
            CraftDailyCornerContext context)
        {
            _http = http;
            _context = context;
        }

        //Session 取購物車
        private const string CART_KEY = "CART";

        public List<VMCartItem> GetSessionCart()
        {
            var session = _http.HttpContext!.Session;
            var json = session.GetString(CART_KEY);

            return json == null
                ? new List<VMCartItem>()
                : JsonSerializer.Deserialize<List<VMCartItem>>(json)!;
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

            foreach (var item in sessionCart)
            {
                var dbItem = _context.CartItems
                    .Include(ci => ci.Cart)
                    .FirstOrDefault(ci =>
                        ci.Cart.MemberID == memberId &&
                        ci.ProductID == item.ProductID);

                if (dbItem == null)
                {
                    _context.CartItems.Add(new CartItem
                    {
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
            var items = _context.CartItems
                .Include(ci => ci.Cart)
                .Where(c => c.Cart.MemberID == memberId)
                .Select(c => new VMCartItem
                {
                    ProductID = c.ProductID,
                    Quantity = c.Quantity
                })
                .ToList();

            SetSessionCart(items);
        }

    }
}

using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CraftDailyCorner.Services
{
    public class CartService
    {
        private readonly IHttpContextAccessor _http;
        private readonly CraftDailyCornerContext _context;

        private const string CART_KEY = "CART";

        public CartService(
            IHttpContextAccessor http,
            CraftDailyCornerContext context)
        {
            _http = http;
            _context = context;
        }
        // 對外唯一入口：加入購物車

        public CartResult AddToCart(string productId, int qty, bool isAuthenticated, string? memberId)
        {
            if (string.IsNullOrWhiteSpace(productId) || qty <= 0)
            {
                return Fail("商品或數量不正確");
            }

            // 1️ 取得商品 + Inventory
            var product = _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductImages)
                .FirstOrDefault(p =>
                    p.ProductID == productId &&
                    p.StatusID == 2); // 上架

            if (product == null)
                return Fail("商品不存在或未上架");

            var stockQty = product.Inventory?.StockQty ?? (short)0;

            if (stockQty <= 0)
                return Fail("商品已缺貨", stockQty);

            // 2️ 已登入 → DB Cart

            if (isAuthenticated && !string.IsNullOrEmpty(memberId))
            {
                var cart = GetOrCreateCart(memberId);

                var cartItem = _context.CartItems
                    .FirstOrDefault(ci =>
                        ci.CartID == cart.CartID &&
                        ci.ProductID == productId);

                var currentQty = cartItem?.Quantity ?? 0;
                var newQty = currentQty + qty;

                if (newQty > stockQty)
                    return Fail("加入後數量超過庫存", stockQty);

                if (cartItem == null)
                {
                    _context.CartItems.Add(new CartItem
                    {
                        CartID = cart.CartID,
                        ProductID = productId,
                        Quantity = (short)newQty,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    });
                }
                else
                {
                    cartItem.Quantity = (short)newQty;
                    cartItem.UpdatedAt = DateTime.Now;
                }

                _context.SaveChanges();

                return Success(GetDbCartQty(cart.CartID), stockQty);
            }

            // 3️ 未登入 → Session Cart
            var sessionCart = GetSessionCart();

            var sessionItem = sessionCart
                .FirstOrDefault(i => i.ProductID == productId);

            var sessionQty = sessionItem?.Quantity ?? 0;
            var newSessionQty = sessionQty + qty;

            if (newSessionQty > stockQty)
                return Fail("加入後數量超過庫存", stockQty);

            if (sessionItem == null)
            {
                sessionCart.Add(new VMCartItem
                {
                    ProductID = product.ProductID,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    Quantity = (short)newSessionQty,
                    ImageUrl = product.ProductImages
                        .Where(i => i.StatusID == 1)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                });
            }
            else
            {
                sessionItem.Quantity = (short)newSessionQty;
            }

            SetSessionCart(sessionCart);

            return Success(sessionCart.Sum(i => i.Quantity), stockQty);
        }


        //  Helper Methods（私有）


        private Cart GetOrCreateCart(string memberId)
        {
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

        private int GetDbCartQty(int cartId)
        {
            return _context.CartItems
                .Where(ci => ci.CartID == cartId)
                .Sum(ci => ci.Quantity);
        }

        private List<VMCartItem> GetSessionCart()
        {
            var session = _http.HttpContext!.Session;
            var json = session.GetString(CART_KEY);

            return json == null
                ? new List<VMCartItem>()
                : JsonSerializer.Deserialize<List<VMCartItem>>(json)!;
        }

        private void SetSessionCart(List<VMCartItem> cart)
        {
            var json = JsonSerializer.Serialize(cart);
            _http.HttpContext!.Session.SetString(CART_KEY, json);
        }

        private CartResult Fail(string message, short stockQty = 0)
        {
            return new CartResult
            {
                Success = false,
                Message = message,
                StockQty = stockQty
            };
        }

        private CartResult Success(int cartQty, short stockQty)
        {
            return new CartResult
            {
                Success = true,
                Message = "加入購物車成功",
                CartQty = cartQty,
                StockQty = stockQty
            };
        }
        public CartResult RemoveFromCart(
            string productId,
            bool isAuthenticated,
            string? memberId)
        {
            if (string.IsNullOrWhiteSpace(productId))
                return Fail("商品資料錯誤");

            // 已登入 → DB
            if (isAuthenticated && !string.IsNullOrEmpty(memberId))
            {
                var cart = _context.Carts
                    .FirstOrDefault(c => c.MemberID == memberId);

                if (cart == null)
                    return Success(0, 0);

                var item = _context.CartItems
                    .FirstOrDefault(ci =>
                        ci.CartID == cart.CartID &&
                        ci.ProductID == productId);

                if (item != null)
                {
                    _context.CartItems.Remove(item);
                    _context.SaveChanges();
                }

                var cartQty = GetDbCartQty(cart.CartID);
                return Success(cartQty, 0);
            }

            // 未登入 → Session
            var sessionCart = GetSessionCart();

            var sessionItem = sessionCart
                .FirstOrDefault(i => i.ProductID == productId);

            if (sessionItem != null)
            {
                sessionCart.Remove(sessionItem);
                SetSessionCart(sessionCart);
            }

            return Success(sessionCart.Sum(i => i.Quantity), 0);
        }
        public int GetCartCount(bool isAuthenticated, string? memberId)
        {
            // 已登入 → DB
            if (isAuthenticated && !string.IsNullOrEmpty(memberId))
            {
                var cart = _context.Carts
                    .FirstOrDefault(c => c.MemberID == memberId);

                if (cart == null)
                    return 0;

                return GetDbCartQty(cart.CartID);
            }

            // 未登入 → Session
            return GetSessionCart().Sum(i => i.Quantity);
        }

        // 登入成功後：Session → DB
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
                        CreatedAt = DateTime.Now,
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
        }

        // 清空 Session Cart
        public void ClearSessionCart()
        {
            _http.HttpContext!.Session.Remove(CART_KEY);
        }

        public List<VMCartItem> GetCartItemsForCheckout(string memberId)
        {
            var cart = _context.Carts
                .FirstOrDefault(c => c.MemberID == memberId);

            if (cart == null)
                return new List<VMCartItem>();

            return _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(ci => ci.CartID == cart.CartID)
                .Select(ci => new VMCartItem
                {
                    ProductID = ci.ProductID,
                    ProductName = ci.Product.ProductName,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product.ProductImages
                        .Where(i => i.StatusID == 1)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();
        }

    }
}

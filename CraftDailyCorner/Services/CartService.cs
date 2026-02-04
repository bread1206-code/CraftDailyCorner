//using CraftDailyCorner.Models;
//using CraftDailyCorner.ViewModels.Front;
//using Microsoft.EntityFrameworkCore;
//using static System.Net.WebRequestMethods;

//namespace CraftDailyCorner.Services
//{
//    public class CartService
//    {
//        private readonly CraftDailyCornerContext _context;
//        private readonly IHttpContextAccessor _http;

//        private const string CART_KEY = "CART";


//        public CartService(CraftDailyCornerContext context, IHttpContextAccessor http)
//        {
//            _context = context;
//            _http = http;
//        }

//        // 加入購物車（可指定數量）
//        public void AddItem(string memberId, string productId, int quantity)
//        {
//            if (quantity <= 0) quantity = 1;

//            var cart = _context.Carts
//                .Include(c => c.CartItems)
//                .FirstOrDefault(c => c.MemberID == memberId);

//            if (cart == null)
//            {
//                cart = new Cart
//                {
//                    MemberID = memberId,
//                    CreatedAt = DateTime.Now
//                };
//                _context.Carts.Add(cart);
//            }

//            var item = cart.CartItems
//                .FirstOrDefault(i => i.ProductID == productId);

//            if (item == null)
//            {
//                cart.CartItems.Add(new CartItem
//                {
//                    ProductID = productId,
//                    Quantity = quantity
//                });
//            }
//            else
//            {
//                item.Quantity += quantity;
//            }

//            _context.SaveChanges();
//        }

//        // 移除商品
//        public void RemoveItem(string memberId, string productId)
//        {
//            var item = _context.CartItems
//                .Include(i => i.Cart)
//                .FirstOrDefault(i =>
//                    i.ProductID == productId &&
//                    i.Cart.MemberID == memberId);

//            if (item == null) return;

//            _context.CartItems.Remove(item);
//            _context.SaveChanges();
//        }

//        // 取得購物車內容（給 ViewComponent / Modal）
//        public List<VMCartItem> GetCartItem(string memberId)
//        {
//            return _context.CartItems
//                .Where(i => i.Cart.MemberID == memberId)
//                .Include(i => i.Product)
//                .Include(i=> i.Product.ProductImages)
//                .Select(i => new VMCartItem
//                {
//                    ProductId = i.ProductID,
//                    ProductName = i.Product.ProductName,
//                    ImageUrl = i.Product.ProductImages.FirstOrDefault().ImageUrl,
//                    Quantity = i.Quantity,
//                    Price = i.Product.Price,
//                    CartId = i.CartID
//                })
//                .ToList();
//        }

//        // 取得購物車商品總數（Badge 用）
//        public int GetCartCount(string memberId)
//        {
//            return _context.CartItems
//                .Where(i => i.Cart.MemberID == memberId)
//                .Sum(i => i.Quantity);
//        }
//        //清空 Session（登出 / 結帳後）
//        public void ClearSessionCart()
//        {
//            _http.HttpContext!.Session.Remove(CART_KEY);
//        }
//        //登入後從 DB 還原購物車
//        public void LoadCartFromDb(string memberId)
//        {
//            var cart = GetCartItem(memberId);

//            var items = _context.CartItems
//                .Include(ci => ci.Product)
//                .Where(ci => ci.CartID == cart.CartID)
//                .Select(ci => new VMCartItem
//                {
//                    ProductId = ci.ProductID,
//                    ProductName = ci.Product.ProductName,
//                    Price = ci.Product.Price,
//                    ImageUrl = ci.Product.ProductImages.FirstOrDefault().ImageUrl,
//                    Quantity = ci.Quantity
//                })
//                .ToList();

//            SetSessionCart(items);
//        }

//    }
//}

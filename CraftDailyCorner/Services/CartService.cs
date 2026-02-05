using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CartService
    {
        private readonly CraftDailyCornerContext _context;

        public CartService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        //加入購物車
        public VMCartResult AddItem(string memberId, string productId, int quantity)
        {
            if (quantity <= 0) quantity = 1;

            var product = _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductImages)
                .Include(p => p.CreatorProfile)
                .FirstOrDefault(p =>
                    p.ProductID == productId &&
                    p.StatusID == 2);

            if (product == null)
                return Fail("商品不存在或未上架");

            var stockQty = product.Inventory?.StockQty ?? 0;
            if (stockQty <= 0)
                return Fail("商品已缺貨", stockQty);

            var cart = GetOrCreateCart(memberId);

            var item = _context.CartItems
                .FirstOrDefault(ci =>
                    ci.CartID == cart.CartID &&
                    ci.ProductID == productId);

            var newQty = (item?.Quantity ?? 0) + quantity;

            if (newQty > stockQty)
                return Fail("加入後數量超過庫存", stockQty);

            if (item == null)
            {
                _context.CartItems.Add(new CartItem
                {
                    CartID = cart.CartID,
                    ProductID = productId,
                    Quantity = newQty,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });
            }
            else
            {
                item.Quantity = newQty;
                item.UpdatedAt = DateTime.Now;
            }

            _context.SaveChanges();
            return Success(memberId);
        }

        //更新商品數量
        public VMCartResult UpdateQuantity(string memberId, string productId, int quantity)
        {
            if (quantity <= 0)
                return Fail("數量必須大於 0");

            var cart = GetCart(memberId);
            if (cart == null)
                return Fail("購物車不存在");

            var item = _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.Inventory)
                .FirstOrDefault(ci =>
                    ci.CartID == cart.CartID &&
                    ci.ProductID == productId);

            if (item == null)
                return Fail("商品不存在於購物車");

            var stockQty = item.Product.Inventory?.StockQty ?? 0;
            if (quantity > stockQty)
                return Fail("數量超過庫存", stockQty);

            item.Quantity = quantity;
            item.UpdatedAt = DateTime.Now;

            _context.SaveChanges();
            return Success(memberId);
        }

        //移除商品
        public VMCartResult RemoveItem(string memberId, string productId)
        {
            var cart = GetCart(memberId);
            if (cart == null)
                return Success(memberId);

            var item = _context.CartItems
                .FirstOrDefault(ci =>
                    ci.CartID == cart.CartID &&
                    ci.ProductID == productId);

            if (item != null)
            {
                _context.CartItems.Remove(item);
                _context.SaveChanges();
            }

            return Success(memberId);
        }

        //取得購物車清單（Modal / Page）
        public List<VMCartItem> GetCartItems(string memberId)
        {
            var cart = GetCart(memberId);
            if (cart == null)
                return new();

            return _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(ci => ci.CartID == cart.CartID)
                .Select(ci => new VMCartItem
                {
                    ProductId = ci.ProductID,
                    ProductName = ci.Product.ProductName,
                    Price = ci.Product.Price,
                    Quantity = ci.Quantity,
                    ImageUrl = ci.Product.ProductImages
                        .Where(i => i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList();
        }

        //取得購物車商品總數（Badge
        public int GetCartCount(string memberId)
        {
            var cart = GetCart(memberId);
            if (cart == null)
                return 0;

            return _context.CartItems
                .Where(ci => ci.CartID == cart.CartID)
                .Sum(ci => ci.Quantity);
        }

        //Checkout：取得快照商品清單
        public List<VMCheckoutItem> GetCartItemsForCheckout(string memberId)
        {
            var cart = GetCart(memberId);
            if (cart == null)
                return new();

            return _context.CartItems
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.ProductImages)
                .Include(ci => ci.Product)
                    .ThenInclude(p => p.CreatorProfile)
                .Where(ci => ci.CartID == cart.CartID)
                .Select(ci => new VMCheckoutItem
                {
                    Quantity = ci.Quantity,
                    Product = new VMProductSnapshot
                    {
                        ProductId = ci.ProductID,
                        ProductName = ci.Product.ProductName,
                        Price = ci.Product.Price,
                        ImageUrl = ci.Product.ProductImages
                            .Where(i => i.StatusID == 1)
                            .OrderBy(i => i.SortOrder)
                            .Select(i => i.ImageUrl)
                            .FirstOrDefault(),
                        CreatorId = ci.Product.CreatorProfile!.CreatorID,
                        CreatorName = ci.Product.CreatorProfile.DisplayName
                    }
                })
                .ToList();
        }

        //清空購物車（下單成功後）
        public void ClearCart(string memberId)
        {
            var cart = GetCart(memberId);
            if (cart == null) return;

            var items = _context.CartItems
                .Where(ci => ci.CartID == cart.CartID);

            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }

        //Private Helpers
        private Cart GetCart(string memberId)
        {
            return _context.Carts
                .SingleOrDefault(c => c.MemberID == memberId);
        }

        private Cart GetOrCreateCart(string memberId)
        {
            var cart = GetCart(memberId);

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

            return cart;
        }

        private VMCartResult Success(string memberId)
        {
            var items = GetCartItems(memberId);

            return new VMCartResult
            {
                Success = true,
                Message = "操作成功",
                Summary = new VMCartSummary
                {
                    TotalQuantity = items.Sum(i => i.Quantity),
                    TotalAmount = (int)Math.Floor(items.Sum(i => i.SubTotal))
                }
            };
        }

        private VMCartResult Fail(string message, int? stockQty = null)
        {
            return new VMCartResult
            {
                Success = false,
                Message = message,
                StockQty = stockQty
            };
        }
    }
}
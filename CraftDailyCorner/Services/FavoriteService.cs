using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using CraftDailyCorner.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class FavoriteService : IFavoriteService
    {
        private readonly CraftDailyCornerContext _context;

        public FavoriteService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 判斷會員是否已收藏商品
        public bool IsFavorite(string memberId, string productId)
        {
            return _context.FavoriteProducts
                .Any(fp => fp.MemberID == memberId && fp.ProductID == productId);
        }

        // 新增收藏（已存在則忽略）
        public void AddFavorite(string memberId, string productId)
        {
            // Service 層先擋（UX + 效能）
            if (IsFavorite(memberId, productId))
                return;

            var favorite = new FavoriteProduct
            {
                MemberID = memberId,
                ProductID = productId,
                CreatedAt = DateTime.Now
            };

            _context.FavoriteProducts.Add(favorite);

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {
                // 雙保險：資料庫 UNIQUE index 最後防線
                // 發生代表同時請求重複插入，可安全忽略
            }
        }

        // 取消收藏（不存在則忽略）
        public void RemoveFavorite(string memberId, string productId)
        {
            var favorite = _context.FavoriteProducts
                .FirstOrDefault(fp =>
                    fp.MemberID == memberId &&
                    fp.ProductID == productId);

            if (favorite == null)
                return;

            _context.FavoriteProducts.Remove(favorite);
            _context.SaveChanges();
        }

        // 取得會員收藏的商品列表
        public List<VMProductListItem> GetFavoriteProducts(string memberId)
        {
            return _context.FavoriteProducts
                .Where(f => f.MemberID == memberId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.ProductImages)
                .Select(f => new VMProductListItem
                {
                    ProductID = f.Product.ProductID,
                    ProductName = f.Product.ProductName,
                    Price = f.Product.Price,
                    CreatorID = f.Product.CreatorID,

                    CoverImageUrl = f.Product.ProductImages
                        .Where(i => i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault() ?? "no-image.webp",

                    IsFavorite = true // 收藏清單一定是 true
                })
                .ToList();
        }
        //查詢我的收藏
        public List<VMFavoriteProductItem> GetMyFavorites(string memberId)
        {
            return _context.FavoriteProducts
                .Where(f => f.MemberID == memberId)
                .Include(f => f.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(f => f.Product.StatusID == 2) // 上架中
                .Select(f => new VMFavoriteProductItem
                {
                    ProductID = f.ProductID,
                    ProductName = f.Product.ProductName,
                    CreatorID = f.Product.CreatorID,
                    Price = f.Product.Price,
                    CoverImageUrl = f.Product.ProductImages
                        .Where(i => i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault() ?? "no-image.webp",
                    IsFavorite = true
                })
                .ToList();
        }
    }
}
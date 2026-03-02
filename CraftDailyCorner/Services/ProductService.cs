using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Breadcrumb;
using CraftDailyCorner.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class ProductService
    {
        private readonly CraftDailyCornerContext _context;

        public ProductService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        //商品列表（分類 / 搜尋 / Tag）
        public VMProductList GetProductList(int? categoryId,string? keyword,int? tagId,string? memberId)
        {
            var query = _context.Products
                .Include(p => p.ProductImages)
                .Where(p => p.StatusID == 2);

            string pageTitle = "所有商品";

            // 分類
            if (categoryId.HasValue)
            {
                var categoryName = _context.Categories
                    .Where(c => c.CategoryID == categoryId)
                    .Select(c => c.CategoryName)
                    .FirstOrDefault();

                pageTitle = categoryName != null
                    ? $"{categoryName} 類商品"
                    : "分類商品";

                query = query.Where(p =>
                    p.ProductCategories.Any(pc => pc.CategoryID == categoryId));
            }

            // 搜尋（優先於分類）
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                pageTitle = $"搜尋「{keyword}」的結果";

                query = query.Where(p =>
                    p.ProductName.Contains(keyword) ||
                    p.Description.Contains(keyword));
            }

            // 標籤（最高優先）
            if (tagId.HasValue)
            {
                var tagName = _context.Tags
                    .Where(t => t.TagID == tagId)
                    .Select(t => t.TagName)
                    .FirstOrDefault();

                pageTitle = tagName != null
                    ? $"#{tagName} 標籤商品"
                    : "標籤商品";

                query = query.Where(p =>
                    p.ProductTags.Any(pt => pt.TagID == tagId));
            }

            var products = query.ToList();

            // 一次撈會員收藏
            var favoriteIds = new HashSet<string>();

            if (!string.IsNullOrEmpty(memberId))
            {
                favoriteIds = _context.FavoriteProducts
                    .Where(f => f.MemberID == memberId)
                    .Select(f => f.ProductID)
                    .ToHashSet();
            }

            var items = products.Select(p =>
            {
                var cover = p.ProductImages
                    .Where(i => i.StatusID == 1)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .FirstOrDefault() ?? "no-image.png";

                return new VMProductListItem
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    CoverImageUrl = cover,
                    IsFavorite = favoriteIds.Contains(p.ProductID)
                };
            }).ToList();

            return new VMProductList
            {
                Products = items,
                CategoryId = categoryId,
                Keyword = keyword,
                TagId = tagId,
                PageTitle = pageTitle,

                Breadcrumb = BuildProductBreadcrumb(
                    categoryId,
                    keyword,
                    tagId
                )
            };
        }

        //商品詳細頁
        public VMProductDetail? GetProductDetail(string productId, string? memberId)
        {
            var product = _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category)
                .Include(p => p.ProductTags)
                    .ThenInclude(pt => pt.Tag)
                .Include(p => p.CreatorProfile)
                .Include(p => p.Inventory)
                .FirstOrDefault(p => p.ProductID == productId && p.StatusID == 2);

            if (product == null) return null;

            bool isFavorite = false;

            if (!string.IsNullOrEmpty(memberId))
            {
                isFavorite = _context.FavoriteProducts
                    .Any(f => f.MemberID == memberId && f.ProductID == productId);
            }

            return new VMProductDetail
            {
                ProductId = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,

                StockQty = product.Inventory?.StockQty ?? 0,
                AlertQty = product.Inventory?.AlertQty ?? 0,

                ImageUrls = product.ProductImages
                    .Where(i => i.StatusID == 1)
                    .OrderBy(i => i.SortOrder)
                    .Select(i => i.ImageUrl)
                    .ToList(),

                CreatorID = product.CreatorProfile?.CreatorID,
                CreatorName = product.CreatorProfile?.DisplayName,

                Categories = product.ProductCategories
                    .Select(pc => pc.Category)
                    .ToList(),

                Tags = product.ProductTags
                    .Select(pt => pt.Tag)
                    .ToList(),

                IsFavorite = isFavorite,
                IsOwner = memberId != null &&
                              product.CreatorProfile.MemberID == memberId,
                Breadcrumb = BuildProductDetailBreadcrumb(product)
            };
        }
        // 建立商品列表頁的麵包屑導航
        private VMBreadcrumb BuildProductBreadcrumb(int? categoryId,string? keyword,int? tagId)
        {
            var breadcrumb = new VMBreadcrumb();

            // 首頁
            breadcrumb.Items.Add(new VMBreadcrumbItem
            {
                Text = "首頁",
                Url = "/"
            });

            // 商品列表
            breadcrumb.Items.Add(new VMBreadcrumbItem
            {
                Text = "商品",
                Url = "/Products"
            });

            // 搜尋（優先）
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                breadcrumb.Items.Add(new VMBreadcrumbItem
                {
                    Text = $"搜尋「{keyword}」",
                    Url = null
                });

                return breadcrumb;
            }

            // 標籤
            if (tagId.HasValue)
            {
                var tagName = _context.Tags
                    .Where(t => t.TagID == tagId)
                    .Select(t => t.TagName)
                    .FirstOrDefault();

                breadcrumb.Items.Add(new VMBreadcrumbItem
                {
                    Text = tagName != null ? $"#{tagName}" : "標籤",
                    Url = null
                });

                return breadcrumb;
            }

            // 分類
            if (categoryId.HasValue)
            {
                var categoryName = _context.Categories
                    .Where(c => c.CategoryID == categoryId)
                    .Select(c => c.CategoryName)
                    .FirstOrDefault();

                breadcrumb.Items.Add(new VMBreadcrumbItem
                {
                    Text = categoryName ?? "分類商品",
                    Url = null
                });
            }

            return breadcrumb;
        }
        // 建立商品詳細頁的麵包屑導航
        private VMBreadcrumb BuildProductDetailBreadcrumb(Product product)
        {
            var breadcrumb = new VMBreadcrumb();

            // 首頁
            breadcrumb.Items.Add(new VMBreadcrumbItem
            {
                Text = "首頁",
                Url = "/"
            });

            // 商品列表
            breadcrumb.Items.Add(new VMBreadcrumbItem
            {
                Text = "商品",
                Url = "/Products"
            });

            // 分類（取第一個）
            var category = product.ProductCategories
                .Select(pc => pc.Category)
                .FirstOrDefault();

            if (category != null)
            {
                breadcrumb.Items.Add(new VMBreadcrumbItem
                {
                    Text = category.CategoryName,
                    Url = $"/Products?categoryId={category.CategoryID}"
                });
            }

            // 商品名稱（目前頁）
            breadcrumb.Items.Add(new VMBreadcrumbItem
            {
                Text = product.ProductName,
                Url = null
            });

            return breadcrumb;
        }
    }
}
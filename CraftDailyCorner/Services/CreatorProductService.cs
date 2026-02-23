using CraftDailyCorner.ImageManagementCore.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.CreatorProduct;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorProductService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorProductService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // =========================================================
        // 商品列表
        // =========================================================
        public VMCreatorProductList GetCreatorProductList(string creatorId)
        {
            var items = _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductStatus)
                .Where(p => p.CreatorID == creatorId)
                .OrderByDescending(p => p.CreatedAt)
                .Select(p => new VMCreatorProductListItem
                {
                    ProductID = p.ProductID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StatusName = p.ProductStatus.StatusName,
                    StockQty = p.Inventory.StockQty,
                    CreatedAt = p.CreatedAt
                })
                .ToList();

            return new VMCreatorProductList
            {
                Products = items
            };
        }

        // =========================================================
        // 取得建立表單
        // =========================================================
        public VMCreatorProductForm GetCreateForm()
        {
            var vm = new VMCreatorProductForm
            {
                StatusID = 1
            };

            LoadOptions(vm);
            return vm;
        }

        // =========================================================
        // 取得編輯表單
        // =========================================================
        public async Task<VMCreatorProductForm?> GetEditFormAsync(
            string productId,
            string creatorId)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p =>
                    p.ProductID == productId &&
                    p.CreatorID == creatorId);

            if (product == null)
                return null;

            var vm = new VMCreatorProductForm
            {
                ProductID = product.ProductID,
                ProductName = product.ProductName,
                Description = product.Description,
                Price = product.Price,
                StatusID = product.StatusID,
                StockQty = product.Inventory.StockQty,
                AlertQty = product.Inventory.AlertQty,
                SelectedCategoryIds = product.ProductCategories
                    .Select(pc => pc.CategoryID).ToList(),
                SelectedTagIds = product.ProductTags
                    .Select(pt => pt.TagID).ToList()
            };
            vm.ImageManagement = new VMImageManagement
            {
                EntityId = product.ProductID,
                EntityType = "Product"
            };

            LoadOptions(vm);
            return vm;
        }

        // =========================================================
        // 建立商品
        // =========================================================
        public async Task<string> CreateAsync(
            VMCreatorProductForm vm,
            string creatorId)
        {
            var productId = await GenerateProductIdAsync();

            var product = new Product
            {
                ProductID = productId,
                ProductName = vm.ProductName,
                Description = vm.Description,
                Price = vm.Price,
                StatusID = vm.StatusID,
                CreatorID = creatorId,
                CreatedAt = DateTime.Now
            };

            _context.Products.Add(product);

            _context.Inventories.Add(new Inventory
            {
                ProductID = productId,
                StockQty = vm.StockQty,
                AlertQty = vm.AlertQty,
                UpdatedAt = DateTime.Now
            });

            foreach (var cid in vm.SelectedCategoryIds)
            {
                _context.ProductCategories.Add(new ProductCategory
                {
                    ProductID = productId,
                    CategoryID = cid
                });
            }

            foreach (var tid in vm.SelectedTagIds)
            {
                _context.ProductTags.Add(new ProductTag
                {
                    ProductID = productId,
                    TagID = tid
                });
            }

            await _context.SaveChangesAsync();
            return productId;
        }

        // =========================================================
        // 更新商品
        // =========================================================
        public async Task<bool> UpdateAsync(
            VMCreatorProductForm vm,
            string creatorId)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .FirstOrDefaultAsync(p =>
                    p.ProductID == vm.ProductID &&
                    p.CreatorID == creatorId);

            if (product == null)
                return false;

            // ==========================
            // 上架檢查
            // ==========================
            if (vm.StatusID == 2)
            {
                if (product.Inventory.StockQty <= 0)
                    throw new Exception("庫存為 0 無法上架");

                var imageCount = await _context.ProductImages
                    .CountAsync(i =>
                        i.ProductID == product.ProductID &&
                        i.StatusID == 1);

                if (imageCount == 0)
                    throw new Exception("上架商品必須至少一張圖片");
            }

            product.ProductName = vm.ProductName;
            product.Description = vm.Description;
            product.Price = vm.Price;
            product.StatusID = vm.StatusID;

            product.Inventory.StockQty = vm.StockQty;
            product.Inventory.AlertQty = vm.AlertQty;

            _context.ProductCategories.RemoveRange(product.ProductCategories);
            foreach (var cid in vm.SelectedCategoryIds)
            {
                _context.ProductCategories.Add(new ProductCategory
                {
                    ProductID = product.ProductID,
                    CategoryID = cid
                });
            }

            _context.ProductTags.RemoveRange(product.ProductTags);
            foreach (var tid in vm.SelectedTagIds)
            {
                _context.ProductTags.Add(new ProductTag
                {
                    ProductID = product.ProductID,
                    TagID = tid
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }

        // =========================================================
        // 載入選單
        // =========================================================
        public void LoadOptions(VMCreatorProductForm vm)
        {
            vm.StatusSelectList = _context.ProductStatuses
                .Where(s => s.IsActive)
                .Select(s => new SelectListItem
                {
                    Value = s.StatusID.ToString(),
                    Text = s.StatusName,
                    Selected = s.StatusID == vm.StatusID
                }).ToList();

            vm.TagSelectList = _context.Tags
                .Where(t => t.IsActive)
                .Select(t => new SelectListItem
                {
                    Value = t.TagID.ToString(),
                    Text = t.TagName,
                    Selected = vm.SelectedTagIds.Contains(t.TagID)
                }).ToList();

            var allCategories = _context.Categories
                .Where(c => c.IsActive)
                .ToList();

            var parents = allCategories
                .Where(c => c.ParentCategoryID == null)
                .ToList();

            vm.CategoryGroups = parents.Select(p => new VMCategoryGroup
            {
                ParentCategoryName = p.CategoryName,
                Children = allCategories
                    .Where(c => c.ParentCategoryID == p.CategoryID)
                    .Select(c => new VMCategoryChild
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName,
                        IsSelected = vm.SelectedCategoryIds.Contains(c.CategoryID)
                    }).ToList()
            }).ToList();
        }

        private async Task<string> GenerateProductIdAsync()
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();

            command.CommandText = "getCreatedProductID";
            command.CommandType = System.Data.CommandType.StoredProcedure;

            var outputParam = command.CreateParameter();
            outputParam.ParameterName = "@NewProductID";
            outputParam.DbType = System.Data.DbType.String;
            outputParam.Size = 10;
            outputParam.Direction = System.Data.ParameterDirection.Output;

            command.Parameters.Add(outputParam);

            await _context.Database.OpenConnectionAsync();
            await command.ExecuteNonQueryAsync();
            await _context.Database.CloseConnectionAsync();

            return outputParam.Value?.ToString()
                   ?? throw new Exception("無法取得商品編號");
        }
    }
}
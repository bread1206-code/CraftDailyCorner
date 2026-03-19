using CraftDailyCorner.DTOs;
using CraftDailyCorner.ImageManagementCore.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorProduct;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorProductService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly INotificationService _notificationService;

        public CreatorProductService(
            CraftDailyCornerContext context,
            INotificationService notificationService)
        {
            _context = context;
            _notificationService = notificationService;
        }

        // 商品列表
        public VMCreatorProductList GetCreatorProductList(string creatorId)
        {
            var items = _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductStatus)
                .Include(p => p.ProductImages)
                .Where(p => p.CreatorID == creatorId)
                .OrderBy(p =>
                    p.Inventory.StockQty == 0 ? 0 :
                    p.Inventory.StockQty <= p.Inventory.AlertQty ? 1 : 2)
                .ThenByDescending(p => p.ProductID)
                .Select(p => new VMCreatorProductListItem
                {
                    ProductID = p.ProductID,
                    CreatorID = p.CreatorID,
                    ProductName = p.ProductName,
                    Price = p.Price,
                    StatusID = p.StatusID,
                    StatusName = p.ProductStatus.StatusName,
                    StockQty = p.Inventory.StockQty,
                    CoverImageUrl = p.ProductImages
                        .Where(i => i.StatusID == 1)
                        .OrderBy(i => i.SortOrder)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault(),
                    AlertQty = p.Inventory.AlertQty,
                    CreatedAt = p.CreatedAt
                })
                .ToList();

            return new VMCreatorProductList
            {
                Products = items
            };
        }

        // 取得建立表單
        public VMCreatorProductForm GetCreateForm()
        {
            var vm = new VMCreatorProductForm
            {
                StatusID = 1
            };

            LoadOptions(vm);
            return vm;
        }

        // 取得編輯表單
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
                CostPrice = product.CostPrice,
                StatusID = product.StatusID,
                StockQty = product.Inventory.StockQty,
                AlertQty = product.Inventory.AlertQty,
                SelectedCategoryIds = product.ProductCategories
                    .Select(pc => pc.CategoryID)
                    .ToList(),
                SelectedTagIds = product.ProductTags
                    .Select(pt => pt.TagID)
                    .ToList()
            };

            vm.ImageManagement = new VMImageManagement
            {
                EntityId = product.ProductID,
                EntityType = "Product"
            };

            LoadOptions(vm);
            return vm;
        }

        // 建立商品
        public async Task<string> CreateAsync(
            VMCreatorProductForm vm,
            string creatorId)
        {
            var productId = await GenerateProductIdAsync();
            var now = DateTime.Now;

            var product = new Product
            {
                ProductID = productId,
                ProductName = vm.ProductName,
                Description = vm.Description,
                Price = vm.Price,
                CostPrice = vm.CostPrice,
                StatusID = vm.StatusID,
                CreatorID = creatorId,
                CreatedAt = now
            };

            _context.Products.Add(product);

            _context.Inventories.Add(new Inventory
            {
                ProductID = productId,
                StockQty = vm.StockQty,
                AlertQty = vm.AlertQty,
                UpdatedAt = now
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

            // 新商品通知：只有新建且直接上架才通知追蹤者
            if (vm.StatusID == 2)
            {
                var followerMemberIds = await _context.FollowCreators
                    .Where(x => x.CreatorID == creatorId)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (followerMemberIds.Any())
                {
                    var dtos = followerMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.CreatorNewProduct,
                        Title = "創作者新商品通知",
                        Content = $"你追蹤的創作者上架了新商品「{vm.ProductName}」。",
                        LinkUrl = $"/Products/Detail/{productId}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = productId
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }

            var creatorMemberId = await _context.CreatorProfiles
                .Where(x => x.CreatorID == creatorId)
                .Select(x => x.MemberID)
                .FirstOrDefaultAsync();

            if (!string.IsNullOrWhiteSpace(creatorMemberId))
            {
                if (vm.StockQty == 0)
                {
                    await _notificationService.CreateAsync(new CreateNotificationDTO
                    {
                        MemberID = creatorMemberId,
                        NotificationType = NotificationType.ProductOutOfStock,
                        Title = "商品缺貨通知",
                        Content = $"商品「{vm.ProductName}」目前已缺貨。",
                        LinkUrl = $"/CreatorProducts/Edit/{productId}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = productId
                    });
                }
                else if (vm.StockQty <= vm.AlertQty)
                {
                    await _notificationService.CreateAsync(new CreateNotificationDTO
                    {
                        MemberID = creatorMemberId,
                        NotificationType = NotificationType.ProductLowStock,
                        Title = "商品低庫存通知",
                        Content = $"商品「{vm.ProductName}」目前庫存僅剩 {vm.StockQty} 件，已達警戒值。",
                        LinkUrl = $"/CreatorProducts/Edit/{productId}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = productId
                    });
                }
            }

            return productId;
        }

        // 更新商品
        public async Task<bool> UpdateAsync(
            VMCreatorProductForm vm,
            string creatorId)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductTags)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p =>
                    p.ProductID == vm.ProductID &&
                    p.CreatorID == creatorId);

            if (product == null)
                return false;

            var oldStatusId = product.StatusID;
            var oldStockQty = product.Inventory?.StockQty ?? 0;

            if (vm.StatusID == 2)
            {
                if (vm.StockQty <= 0)
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
            product.CostPrice = vm.CostPrice;
            product.StatusID = vm.StatusID;

            product.Inventory!.StockQty = vm.StockQty;
            product.Inventory.AlertQty = vm.AlertQty;
            product.Inventory.UpdatedAt = DateTime.Now;

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

            var creatorMemberId = await _context.CreatorProfiles
                .Where(x => x.CreatorID == creatorId)
                .Select(x => x.MemberID)
                .FirstOrDefaultAsync();

            // 收藏商品已上架：只在第一次從非上架 -> 上架時發
            if (oldStatusId != 2 && vm.StatusID == 2)
            {
                var favoriteMemberIds = await _context.FavoriteProducts
                    .Where(x => x.ProductID == product.ProductID)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (favoriteMemberIds.Any())
                {
                    var dtos = favoriteMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.FavoriteProductPublished,
                        Title = "收藏商品已上架通知",
                        Content = $"你收藏的商品「{product.ProductName}」已上架。",
                        LinkUrl = $"/Products/Detail/{product.ProductID}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = product.ProductID
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }

            // 收藏商品補貨
            if (oldStockQty == 0 && vm.StockQty > 0)
            {
                var favoriteMemberIds = await _context.FavoriteProducts
                    .Where(x => x.ProductID == product.ProductID)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (favoriteMemberIds.Any())
                {
                    var dtos = favoriteMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.FavoriteProductRestocked,
                        Title = "收藏商品已補貨通知",
                        Content = $"你收藏的商品「{product.ProductName}」已補貨，可以購買了。",
                        LinkUrl = $"/Products/Detail/{product.ProductID}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = product.ProductID
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }

            if (!string.IsNullOrWhiteSpace(creatorMemberId))
            {
                // 低庫存通知
                if (oldStockQty > vm.AlertQty && vm.StockQty > 0 && vm.StockQty <= vm.AlertQty)
                {
                    await _notificationService.CreateAsync(new CreateNotificationDTO
                    {
                        MemberID = creatorMemberId,
                        NotificationType = NotificationType.ProductLowStock,
                        Title = "商品低庫存通知",
                        Content = $"商品「{product.ProductName}」目前庫存僅剩 {vm.StockQty} 件，已達警戒值。",
                        LinkUrl = $"/CreatorProducts/Edit/{product.ProductID}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = product.ProductID
                    });
                }

                // 缺貨通知
                if (oldStockQty > 0 && vm.StockQty == 0)
                {
                    await _notificationService.CreateAsync(new CreateNotificationDTO
                    {
                        MemberID = creatorMemberId,
                        NotificationType = NotificationType.ProductOutOfStock,
                        Title = "商品缺貨通知",
                        Content = $"商品「{product.ProductName}」目前已缺貨。",
                        LinkUrl = $"/CreatorProducts/Edit/{product.ProductID}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = product.ProductID
                    });
                }
            }

            return true;
        }

        // 快速切換上架 / 下架
        public async Task TogglePublishStatusAsync(string productId, string creatorId)
        {
            var product = await _context.Products
                .Include(p => p.Inventory)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p =>
                    p.ProductID == productId &&
                    p.CreatorID == creatorId);

            if (product == null)
                throw new Exception("找不到商品資料");

            var oldStatusId = product.StatusID;

            // 目前是上架中 => 改成下架
            if (product.StatusID == 2)
            {
                product.StatusID = 3;
                await _context.SaveChangesAsync();
                return;
            }

            // 草稿 / 下架 => 改成上架
            if (product.Inventory == null)
                throw new Exception("商品庫存資料不存在，無法上架");

            if (product.Inventory.StockQty <= 0)
                throw new Exception("庫存為 0 無法上架");

            var imageCount = product.ProductImages.Count(i => i.StatusID == 1);
            if (imageCount == 0)
                throw new Exception("上架商品必須至少一張圖片");

            product.StatusID = 2;
            await _context.SaveChangesAsync();

            // 收藏商品已上架通知：只在非上架 -> 上架時發
            if (oldStatusId != 2)
            {
                var favoriteMemberIds = await _context.FavoriteProducts
                    .Where(x => x.ProductID == product.ProductID)
                    .Select(x => x.MemberID)
                    .Distinct()
                    .ToListAsync();

                if (favoriteMemberIds.Any())
                {
                    var dtos = favoriteMemberIds.Select(memberId => new CreateNotificationDTO
                    {
                        MemberID = memberId,
                        NotificationType = NotificationType.FavoriteProductPublished,
                        Title = "收藏商品已上架通知",
                        Content = $"你收藏的商品「{product.ProductName}」已上架。",
                        LinkUrl = $"/Products/Detail/{product.ProductID}",
                        RelatedEntityType = "Product",
                        RelatedEntityId = product.ProductID
                    });

                    await _notificationService.CreateBatchAsync(dtos);
                }
            }
        }

        // 載入選單
        public void LoadOptions(VMCreatorProductForm vm)
        {
            vm.StatusSelectList = _context.ProductStatuses
                .Where(s => s.IsActive)
                .Select(s => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = s.StatusID.ToString(),
                    Text = s.StatusName,
                    Selected = s.StatusID == vm.StatusID
                })
                .ToList();

            vm.TagSelectList = _context.Tags
                .Where(t => t.IsActive)
                .Select(t => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
                {
                    Value = t.TagID.ToString(),
                    Text = t.TagName,
                    Selected = vm.SelectedTagIds.Contains(t.TagID)
                })
                .ToList();

            var allCategories = _context.Categories
                .Where(c => c.IsActive)
                .ToList();

            var parents = allCategories
                .Where(c => c.ParentCategoryID == null)
                .ToList();

            vm.CategoryGroups = parents
                .Select(p => new VMCategoryGroup
                {
                    ParentCategoryName = p.CategoryName,
                    Children = allCategories
                        .Where(c => c.ParentCategoryID == p.CategoryID)
                        .Select(c => new VMCategoryChild
                        {
                            CategoryID = c.CategoryID,
                            CategoryName = c.CategoryName,
                            IsSelected = vm.SelectedCategoryIds.Contains(c.CategoryID)
                        })
                        .ToList()
                })
                .ToList();
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
using CraftDailyCorner.Areas.Admin.ViewModels.Category;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminCategoryService : IAdminCategoryService
    {
        private readonly CraftDailyCornerContext _context;

        public AdminCategoryService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminCategoryIndex> GetIndexAsync()
        {
            var all = await _context.Categories
                .AsNoTracking()
                .OrderBy(c => c.ParentCategoryID == null ? 0 : 1)
                .ThenBy(c => c.ParentCategoryID)
                .ThenBy(c => c.CategoryID)
                .ToListAsync();

            var parents = all.Where(x => x.ParentCategoryID == null)
                             .OrderBy(x => x.CategoryID)
                             .ToList();

            var vm = new VMAdminCategoryIndex();

            foreach (var p in parents)
            {
                vm.Items.Add(new VMAdminCategoryIndexItem
                {
                    CategoryID = p.CategoryID,
                    CategoryName = p.CategoryName,
                    ParentCategoryID = null,
                    ParentCategoryName = null,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    Level = 0
                });

                var children = all.Where(x => x.ParentCategoryID == p.CategoryID)
                                  .OrderBy(x => x.CategoryID)
                                  .ToList();

                foreach (var c in children)
                {
                    vm.Items.Add(new VMAdminCategoryIndexItem
                    {
                        CategoryID = c.CategoryID,
                        CategoryName = c.CategoryName,
                        ParentCategoryID = c.ParentCategoryID,
                        ParentCategoryName = p.CategoryName,
                        IsActive = c.IsActive,
                        CreatedAt = c.CreatedAt,
                        Level = 1
                    });
                }
            }

            var orphanChildren = all
                .Where(x => x.ParentCategoryID != null && !parents.Any(p => p.CategoryID == x.ParentCategoryID))
                .OrderBy(x => x.ParentCategoryID)
                .ThenBy(x => x.CategoryID)
                .ToList();

            foreach (var o in orphanChildren)
            {
                vm.Items.Add(new VMAdminCategoryIndexItem
                {
                    CategoryID = o.CategoryID,
                    CategoryName = o.CategoryName,
                    ParentCategoryID = o.ParentCategoryID,
                    ParentCategoryName = $"（不存在：{o.ParentCategoryID}）",
                    IsActive = o.IsActive,
                    CreatedAt = o.CreatedAt,
                    Level = 1
                });
            }

            return vm;
        }

        public async Task<VMAdminCategoryUpsert> GetCreateVmAsync()
        {
            var vm = new VMAdminCategoryUpsert
            {
                IsActive = true
            };

            await FillParentOptionsAsync(vm, excludeCategoryId: null);
            return vm;
        }

        public async Task CreateAsync(VMAdminCategoryUpsert vm)
        {
            var entity = new Category
            {
                CategoryName = vm.CategoryName.Trim(),
                ParentCategoryID = vm.ParentCategoryID,
                IsActive = vm.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Categories.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<VMAdminCategoryUpsert?> GetEditVmAsync(int id)
        {
            var entity = await _context.Categories
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryID == id);

            if (entity == null) return null;

            var vm = new VMAdminCategoryUpsert
            {
                CategoryID = entity.CategoryID,
                CategoryName = entity.CategoryName,
                ParentCategoryID = entity.ParentCategoryID,
                IsActive = entity.IsActive
            };

            await FillParentOptionsAsync(vm, excludeCategoryId: entity.CategoryID);
            return vm;
        }

        public async Task<bool> UpdateAsync(VMAdminCategoryUpsert vm)
        {
            if (vm.CategoryID == null) return false;

            var entity = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryID == vm.CategoryID.Value);

            if (entity == null) return false;

            if (vm.ParentCategoryID == entity.CategoryID)
                vm.ParentCategoryID = null;

            entity.CategoryName = vm.CategoryName.Trim();
            entity.ParentCategoryID = vm.ParentCategoryID;
            entity.IsActive = vm.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        // =============================
        // Soft Delete = Disable (IsActive = false)
        // =============================
        public async Task<(bool ok, string? message)> DisableAsync(int id)
        {
            var entity = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryID == id);

            if (entity == null)
                return (false, "找不到分類");

            if (!entity.IsActive)
                return (false, "此分類已是停用狀態");

            // 若是大分類，且底下有「仍啟用」的小分類 -> 不讓停用（避免前台分類變怪）
            if (entity.ParentCategoryID == null)
            {
                var hasActiveChildren = await _context.Categories
                    .AnyAsync(x => x.ParentCategoryID == id && x.IsActive);

                if (hasActiveChildren)
                    return (false, "此大分類底下仍有啟用的小分類，請先停用小分類後再停用大分類。");
            }

            entity.IsActive = false;
            await _context.SaveChangesAsync();

            return (true, null);
        }

        // =============================
        // Restore = Enable (IsActive = true)
        // =============================
        public async Task<(bool ok, string? message)> EnableAsync(int id)
        {
            var entity = await _context.Categories
                .FirstOrDefaultAsync(x => x.CategoryID == id);

            if (entity == null)
                return (false, "找不到分類");

            if (entity.IsActive)
                return (false, "此分類已是啟用狀態");

            // 若要啟用小分類，父分類必須存在且啟用
            if (entity.ParentCategoryID != null)
            {
                var parent = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.CategoryID == entity.ParentCategoryID.Value);

                if (parent == null)
                    return (false, "無法啟用：上層大分類不存在");

                if (!parent.IsActive)
                    return (false, "無法啟用：上層大分類目前停用中，請先啟用大分類");
            }

            entity.IsActive = true;
            await _context.SaveChangesAsync();

            return (true, null);
        }

        private async Task FillParentOptionsAsync(VMAdminCategoryUpsert vm, int? excludeCategoryId)
        {
            var parents = await _context.Categories
                .AsNoTracking()
                .Where(c => c.ParentCategoryID == null)
                .Where(c => excludeCategoryId == null || c.CategoryID != excludeCategoryId.Value)
                .OrderBy(c => c.CategoryID)
                .Select(c => new { c.CategoryID, c.CategoryName })
                .ToListAsync();

            var list = parents
                .Select(x => new SelectListItem { Value = x.CategoryID.ToString(), Text = x.CategoryName })
                .ToList();

            list.Insert(0, new SelectListItem { Value = "", Text = "（建立/設為大分類）" });

            vm.ParentCategoryOptions = new SelectList(list, "Value", "Text",
                vm.ParentCategoryID?.ToString() ?? "");
        }
    }
}
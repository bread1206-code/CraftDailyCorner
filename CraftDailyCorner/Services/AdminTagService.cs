using CraftDailyCorner.Areas.Admin.ViewModels.Tag;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminTagService : IAdminTagService
    {
        private readonly CraftDailyCornerContext _context;

        public AdminTagService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminTagIndex> GetIndexAsync()
        {
            var items = await _context.Tags
                .AsNoTracking()
                .OrderByDescending(t => t.IsActive)
                .ThenBy(t => t.TagID)
                .Select(t => new VMAdminTagIndexItem
                {
                    TagID = t.TagID,
                    TagName = t.TagName,
                    IsActive = t.IsActive
                })
                .ToListAsync();

            return new VMAdminTagIndex { Items = items };
        }

        public async Task CreateAsync(VMAdminTagEdit vm)
        {
            var name = (vm.TagName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("標籤名稱不可為空");

            var exists = await _context.Tags
                .AnyAsync(t => t.TagName == name);
            if (exists)
                throw new ArgumentException("此標籤名稱已存在");

            var tag = new Tag
            {
                TagName = name,
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
        }

        public async Task<VMAdminTagEdit?> GetEditAsync(int id)
        {
            return await _context.Tags
                .AsNoTracking()
                .Where(t => t.TagID == id)
                .Select(t => new VMAdminTagEdit
                {
                    TagID = t.TagID,
                    TagName = t.TagName
                })
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateAsync(VMAdminTagEdit vm)
        {
            var name = (vm.TagName ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("標籤名稱不可為空");

            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagID == vm.TagID);

            if (tag == null) return false;

            var nameExists = await _context.Tags
                .AnyAsync(t => t.TagID != vm.TagID && t.TagName == name);

            if (nameExists)
                throw new ArgumentException("此標籤名稱已存在");

            tag.TagName = name;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ToggleActiveAsync(int id)
        {
            var tag = await _context.Tags
                .FirstOrDefaultAsync(t => t.TagID == id);

            if (tag == null) return false;

            tag.IsActive = !tag.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
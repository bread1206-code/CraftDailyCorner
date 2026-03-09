using CraftDailyCorner.Areas.Admin.ViewModels.HomepageBanner;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminHomepageBannerService : IAdminHomepageBannerService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        private const byte STATUS_ACTIVE = 1;
        private const byte STATUS_INACTIVE = 2;

        public AdminHomepageBannerService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        public async Task<VMAdminHomepageBannerIndex> GetIndexAsync()
        {
            var items = await _context.HomepageBanners
                .AsNoTracking()
                .Include(x => x.HomepageBannerStatus)
                .Include(x => x.Member)
                .OrderBy(x => x.StatusID == STATUS_ACTIVE ? 0 : 1)
                .ThenByDescending(x => x.BannerID)
                .Select(x => new VMAdminHomepageBannerIndexItem
                {
                    BannerID = x.BannerID,
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    ImageUrl = x.ImageUrl,
                    StatusID = x.StatusID,
                    StatusName = x.HomepageBannerStatus.StatusName,
                    CreatedAt = x.CreatedAt,
                    CreatedBy = x.CreatedBy,
                    CreatedByName = x.Member.DisplayName
                })
                .ToListAsync();

            return new VMAdminHomepageBannerIndex
            {
                Items = items
            };
        }

        public Task<VMAdminHomepageBannerUpsert> GetCreateVmAsync()
        {
            return Task.FromResult(new VMAdminHomepageBannerUpsert
            {
                StatusID = STATUS_ACTIVE
            });
        }

        public async Task<VMAdminHomepageBannerUpsert?> GetEditVmAsync(int id)
        {
            return await _context.HomepageBanners
                .AsNoTracking()
                .Where(x => x.BannerID == id)
                .Select(x => new VMAdminHomepageBannerUpsert
                {
                    BannerID = x.BannerID,
                    Title = x.Title,
                    Subtitle = x.Subtitle,
                    CurrentImageUrl = x.ImageUrl,
                    StatusID = x.StatusID
                })
                .FirstOrDefaultAsync();
        }

        public async Task CreateAsync(VMAdminHomepageBannerUpsert vm, string adminMemberId)
        {
            if (vm.ImageFile == null || vm.ImageFile.Length == 0)
                throw new ArgumentException("請上傳輪播圖片");

            var imageKey = Guid.NewGuid().ToString();

            _imageUploadService.UploadImage(
                vm.ImageFile,
                null,
                "08HomepageBanner",
                ImageSizePresets.HomepageBanner,
                imageKey
            );

            var entity = new HomepageBanner
            {
                Title = vm.Title.Trim(),
                Subtitle = string.IsNullOrWhiteSpace(vm.Subtitle) ? null : vm.Subtitle.Trim(),
                ImageUrl = imageKey,
                StatusID = vm.StatusID,
                CreatedAt = DateTime.Now,
                CreatedBy = adminMemberId
            };

            _context.HomepageBanners.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(VMAdminHomepageBannerUpsert vm)
        {
            if (vm.BannerID == null)
                return false;

            var entity = await _context.HomepageBanners
                .FirstOrDefaultAsync(x => x.BannerID == vm.BannerID.Value);

            if (entity == null)
                return false;

            entity.Title = vm.Title.Trim();
            entity.Subtitle = string.IsNullOrWhiteSpace(vm.Subtitle) ? null : vm.Subtitle.Trim();

            if (vm.ImageFile != null && vm.ImageFile.Length > 0)
            {
                var imageKey = entity.ImageUrl;

                _imageUploadService.UploadImage(
                    vm.ImageFile,
                    null,
                    "08HomepageBanner",
                    ImageSizePresets.HomepageBanner,
                    imageKey
                );

                entity.ImageUrl = imageKey;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(bool ok, string? message)> DisableAsync(int id)
        {
            var entity = await _context.HomepageBanners
                .FirstOrDefaultAsync(x => x.BannerID == id);

            if (entity == null)
                return (false, "找不到輪播資料");

            if (entity.StatusID == STATUS_INACTIVE)
                return (false, "此輪播圖已是停用狀態");

            entity.StatusID = STATUS_INACTIVE;
            await _context.SaveChangesAsync();

            return (true, null);
        }

        public async Task<(bool ok, string? message)> EnableAsync(int id)
        {
            var entity = await _context.HomepageBanners
                .FirstOrDefaultAsync(x => x.BannerID == id);

            if (entity == null)
                return (false, "找不到輪播資料");

            if (entity.StatusID == STATUS_ACTIVE)
                return (false, "此輪播圖已是啟用狀態");

            entity.StatusID = STATUS_ACTIVE;
            await _context.SaveChangesAsync();

            return (true, null);
        }
    }
}
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorProfileService : ICreatorProfileService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public CreatorProfileService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        public async Task<VMCreatorBrandEdit?> GetBrandEditAsync(string creatorId)
        {
            var entity = await _context.CreatorProfiles
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CreatorID == creatorId);

            if (entity == null) return null;

            return new VMCreatorBrandEdit
            {
                CreatorID = entity.CreatorID,
                DisplayName = entity.DisplayName, // 唯讀
                ImageUrl = entity.ImageUrl,
                Intro = entity.Intro,
                BankCode = entity.BankCode,
                BankAccount = entity.BankAccount
            };
        }

        public async Task UpdateBrandAsync(string creatorId, VMCreatorBrandEdit vm)
        {
            var entity = await _context.CreatorProfiles
                .FirstOrDefaultAsync(c => c.CreatorID == creatorId);

            if (entity == null)
                throw new Exception("找不到創作者資料");


            entity.Intro = (vm.Intro ?? string.Empty).Trim();
            entity.BankCode = (vm.BankCode ?? string.Empty).Trim();
            entity.BankAccount = (vm.BankAccount ?? string.Empty).Trim();

            // 有上傳才更新圖
            if (vm.BrandImage != null && vm.BrandImage.Length > 0)
            {
                var imageKey = _imageUploadService.UploadImage(
                    vm.BrandImage,
                    null,                   
                    "03CreatorBrand",
                    ImageSizePresets.Creator, 
                    entity.ImageUrl          
                );

                entity.ImageUrl = imageKey;
            }

            await _context.SaveChangesAsync();
        }
    }
}
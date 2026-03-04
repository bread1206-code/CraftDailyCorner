using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorApplication;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorApplicationService : ICreatorApplicationService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorApplicationService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        //取得申請頁應顯示的畫面
        public async Task<object> GetApplyPageAsync(string memberId)
        {
            var latest = await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .Where(ca => ca.MemberID == memberId)
                .OrderByDescending(ca => ca.AppliedAt)
                .FirstOrDefaultAsync();

            // 尚未申請過
            if (latest == null)
            {
                return new VMCreatorApplicationApply
                {
                    StartDate = DateTime.Today
                };
            }

            var statusCode = latest.CreatorApplicationStatus.StatusCode;

            switch (statusCode)
            {
                case "Pending":
                    return new VMCreatorApplicationPending
                    {
                        DisplayName = latest.BrandName,
                        AppliedAt = latest.AppliedAt
                    };

                case "Approved":
                    return new VMCreatorApplicationApproved
                    {
                        DisplayName = latest.BrandName,
                        ReviewedAt = latest.ReviewedAt ?? latest.AppliedAt
                    };

                default:
                    return new VMCreatorApplicationApply
                    {
                        StartDate = DateTime.Today
                    };
            }
        }

        //是否有審核中的申請
        public async Task<bool> HasPendingAsync(string memberId)
        {
            return await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .AnyAsync(ca =>
                    ca.MemberID == memberId &&
                    ca.CreatorApplicationStatus.StatusCode == "Pending");
        }

        //建立申請
        public async Task CreateAsync(CreatorApplicationCreateDTO dto)
        {
            if (await HasPendingAsync(dto.MemberId))
                throw new InvalidOperationException("已有審核中的申請");

            var pendingStatus = await _context.CreatorApplicationStatuses
                .Where(s => s.StatusCode == "Pending" && s.IsActive)
                .FirstOrDefaultAsync();

            if (pendingStatus == null)
                throw new Exception("系統狀態設定錯誤：找不到 Pending 狀態");

            var entity = new CreatorApplication
            {
                MemberID = dto.MemberId,
                BrandName = dto.BrandName,
                BrandIntro = dto.BrandIntro,
                PortfolioSampleUrl = dto.PortfolioSampleUrl,
                StartDate = dto.StartDate,
                AppliedAt = DateTime.Now,
                StatusID = pendingStatus.StatusID
            };

            _context.CreatorApplications.Add(entity);
            await _context.SaveChangesAsync();
        }

        //取得會員最新一筆申請
        public async Task<CreatorApplication?> GetLatestByMemberAsync(string memberId)
        {
            return await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .Where(ca => ca.MemberID == memberId)
                .OrderByDescending(ca => ca.AppliedAt)
                .FirstOrDefaultAsync();
        }
    }
}
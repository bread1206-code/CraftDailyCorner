using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Front.CreatorApplication;
using CraftDailyCorner.ViewModels.Front.DTOs;
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

            if (latest == null)
            {
                return new VMCreatorApplicationApply
                {
                    StartDate = DateTime.Today
                };
            }

            switch (latest.CreatorApplicationStatus.StatusCode)
            {
                case "Pending":
                    return new VMCreatorApplicationPending
                    {
                        DisplayName = latest.DisplayName,
                        AppliedAt = latest.AppliedAt
                    };

                case "Approved":
                    return new VMCreatorApplicationApproved
                    {
                        DisplayName = latest.DisplayName,
                        ReviewedAt = latest.ReviewedAt ?? DateTime.Now
                    };

                default:
                    return new VMCreatorApplicationApply
                    {
                        StartDate = DateTime.Today
                    };
            }
        }

        //是否有 Pending
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

            var pendingStatusId = await _context.CreatorApplicationStatuses
                .Where(s => s.StatusCode == "Pending")
                .Select(s => s.StatusID)
                .FirstOrDefaultAsync();

            if (pendingStatusId == 0)
                throw new Exception("系統狀態設定錯誤：找不到 Pending 狀態");

            var entity = new CreatorApplication
            {
                MemberID = dto.MemberId,
                DisplayName = dto.DisplayName,
                Intro = dto.Intro,
                PortfolioSampleUrl = dto.PortfolioSampleUrl,
                StartDate = dto.StartDate,
                AppliedAt = DateTime.Now,
                StatusID = pendingStatusId
            };

            _context.CreatorApplications.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorApplicationService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorApplicationService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 取得會員最新一筆申請
        public CreatorApplication? GetLatestByMember(string memberId)
        {
            return _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .Where(ca => ca.MemberID == memberId)
                .OrderByDescending(ca => ca.AppliedAt)
                .FirstOrDefault();
        }

        // 是否有審核中的申請
        public bool HasPending(string memberId)
        {
            return _context.CreatorApplications.Any(ca =>
                ca.MemberID == memberId &&
                ca.CreatorApplicationStatus.StatusCode == "Pending");
        }

        // 建立申請（只允許在可申請狀態）
        public void Create(CreatorApplicationCreateDTO dto)
        {
            // 保護性檢查（雙保險）
            if (HasPending(dto.MemberId))
                throw new InvalidOperationException("已有審核中的申請");

            var pendingStatusId = _context.CreatorApplicationStatuses
                .Where(s => s.StatusCode == "Pending")
                .Select(s => s.StatusID)
                .First();

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
            _context.SaveChanges();
        }
    }
}
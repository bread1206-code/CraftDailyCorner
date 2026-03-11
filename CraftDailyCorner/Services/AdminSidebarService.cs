using CraftDailyCorner.Areas.Admin.ViewModels;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class AdminSidebarService : IAdminSidebarService
    {
        private readonly CraftDailyCornerContext _context;

        public AdminSidebarService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMAdminSidebar> GetSidebarDataAsync()
        {
            var pendingCreators = await _context.CreatorApplications
                .Where(x => x.StatusID == 1) // 待審核
                .CountAsync();

            var pendingViolations = await _context.Reports
                .Where(x => x.StatusID == 1) // 未處理
                .CountAsync();

            return new VMAdminSidebar
            {
                PendingCreators = pendingCreators,
                PendingViolations = pendingViolations
            };
        }
    }
}
using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedPostCommentReportStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedPostCommentReportStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.ReportStatuses.Any()) return;

            _context.ReportStatuses.AddRange(
                new ReportStatus
                {
                    StatusID = 1,
                    StatusCode = "Pending",
                    StatusName = "待處理",
                    Description = "尚未審核",
                    IsActive = true
                },
                new ReportStatus
                {
                    StatusID = 2,
                    StatusCode = "Violation",
                    StatusName = "違規",
                    Description = "確認違規",
                    IsActive = false
                },
                new ReportStatus
                {
                    StatusID = 3,
                    StatusCode = "Normal",
                    StatusName = "正常",
                    Description = "確認無違規",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

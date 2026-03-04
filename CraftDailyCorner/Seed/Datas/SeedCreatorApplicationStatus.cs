using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorApplicationStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorApplicationStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.CreatorApplicationStatuses.Any()) return;

            _context.CreatorApplicationStatuses.AddRange(
                new CreatorApplicationStatus
                {
                    StatusID = 1,
                    StatusCode = "Pending",
                    StatusName = "待審核",
                    Description = "申請尚未審核",
                    IsActive = true
                },
                new CreatorApplicationStatus
                {
                    StatusID = 2,
                    StatusCode = "Approved",
                    StatusName = "已通過",
                    Description = "申請已通過",
                    IsActive = true
                },
                new CreatorApplicationStatus
                {
                    StatusID = 3,
                    StatusCode = "Rejected",
                    StatusName = "已拒絕",
                    Description = "申請被拒絕",
                    IsActive = true
                },
                new CreatorApplicationStatus
                {
                    StatusID = 4,
                    StatusCode = "Confirm",
                    StatusName = "已確認",
                    Description = "申請者已確認申請結果",
                    IsActive = true
                }
            );

            _context.SaveChanges();
        }
    }

}

using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorPostStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorPostStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.CreatorPostStatuses.Any()) return;

            _context.CreatorPostStatuses.AddRange(
                new CreatorPostStatus
                {
                    StatusID = 1,
                    StatusCode = "Visible",
                    StatusName = "顯示",
                    Description = "貼文可見",
                    IsActive = true
                },
                new CreatorPostStatus
                {
                    StatusID = 2,
                    StatusCode = "Hidden",
                    StatusName = "隱藏",
                    Description = "貼文被隱藏",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

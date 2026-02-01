using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorProfileStatus
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorProfileStatus(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public void Run()
        {
            if (_context.CreatorProfileStatuses.Any()) return;

            _context.CreatorProfileStatuses.AddRange(
                new CreatorProfileStatus 
                {
                    StatusID = 1,
                    StatusCode = "Active",
                    StatusName = "啟用",
                    Description = "創作者正常啟用",
                    IsActive = true
                },
                new CreatorProfileStatus
                { 
                  StatusID = 2,
                  StatusCode = "Suspended",
                    StatusName = "停權",
                    Description = "創作者帳號停權",
                    IsActive = false
                }
            );

            _context.SaveChanges();
        }
    }

}

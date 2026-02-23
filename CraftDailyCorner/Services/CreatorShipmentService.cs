using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorShipmentService : ICreatorShipmentService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorShipmentService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<string> GenerateTrackingNoAsync()
        {
            var today = DateTime.Now.ToString("yyyyMMdd");
            var prefix = $"SH{today}";

            // 取得今天已存在的最大流水號
            var todayTrackingNos = await _context.Shipments
                .Where(s => s.TrackingNo.StartsWith(prefix))
                .Select(s => s.TrackingNo)
                .ToListAsync();

            int nextNumber = 1;

            if (todayTrackingNos.Any())
            {
                var maxSequence = todayTrackingNos
                    .Select(t =>
                    {
                        var numberPart = t.Substring(prefix.Length);
                        return int.TryParse(numberPart, out int n) ? n : 0;
                    })
                    .Max();

                nextNumber = maxSequence + 1;
            }

            return $"{prefix}{nextNumber.ToString("D4")}";
        }
    }
}

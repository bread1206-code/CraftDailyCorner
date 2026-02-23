using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class FollowService: IFollowService
    {
        private readonly CraftDailyCornerContext _context;

        public FollowService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task ToggleAsync(string creatorId, string memberId)
        {
            var existing = await _context.FollowCreators
                .FirstOrDefaultAsync(f =>
                    f.CreatorID == creatorId &&
                    f.MemberID == memberId);

            if (existing == null)
            {
                _context.FollowCreators.Add(new FollowCreator
                {
                    CreatorID = creatorId,
                    MemberID = memberId,
                    CreatedAt = DateTime.Now
                });
            }
            else
            {
                _context.FollowCreators.Remove(existing);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsFollowingAsync(string creatorId, string memberId)
        {
            return await _context.FollowCreators
                .AnyAsync(f =>
                    f.CreatorID == creatorId &&
                    f.MemberID == memberId);
        }

        public async Task<int> GetFollowerCountAsync(string creatorId)
        {
            return await _context.FollowCreators
                .CountAsync(f => f.CreatorID == creatorId);
        }
    }
}

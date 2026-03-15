using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Homepage;

namespace CraftDailyCorner.ViewComponents
{
    public class VCCreatorPost : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;

        public VCCreatorPost(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var posts = await _context.CreatorPosts
                .AsNoTracking()
                .Where(p => p.StatusID == 1 && p.Visibility == 0)
                .OrderByDescending(p => p.CreatedAt)
                .Take(16)
                .Select(p => new VMHotPostCard
                {
                    PostID = p.PostID,
                    Title = p.Title,
                    ImageUrl = p.ImageUrl,
                    CreatedAt = p.CreatedAt,
                    BrandName = p.CreatorProfile.BrandName,
                    CreatorID = p.CreatorID
                })
                .ToListAsync();

            return View(posts);
        }
    }
}
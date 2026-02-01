using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Front;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.ViewComponents
{
    public class VCProductReview : ViewComponent
    {
        private readonly CraftDailyCornerContext _context;

        public VCProductReview(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string productID)
        {
            var reviews = await _context.ProductReviews
                .Where(r => r.ProductID == productID /*&& r.StatusID == 1*/)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new VMProductReview
                {
                    MemberName = r.Member.DisplayName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt
                })
                .ToListAsync();

            var vm = new VMProductReviewList
            {
                ProductID = productID,
                TotalCount = reviews.Count,
                AvgRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0,
                Reviews = reviews
            };

            return View(vm);
        }
    }

}

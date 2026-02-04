using Microsoft.AspNetCore.Mvc;
using CraftDailyCorner.Models; 
using Microsoft.EntityFrameworkCore;
namespace CraftDailyCorner.ViewComponents
{
    public class VCCategoryDropdown : ViewComponent
    {



        private readonly CraftDailyCornerContext _context;

        public VCCategoryDropdown(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.IsActive)
                .OrderBy(c => c.ParentCategoryID)
                .ThenBy(c => c.CategoryID)
                .ToListAsync();

            return View(categories);
        }
    }




}


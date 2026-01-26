using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models;

namespace CraftDailyCorner.Controllers
{
    public class HomepageBannersController : Controller
    {
        private readonly CraftDailyCornerContext _context;

        public HomepageBannersController(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // GET: HomepageBanners
        public async Task<IActionResult> Index()
        {
            var testContext = _context.HomepageBanner.Include(h => h.Member);
            return View(await testContext.ToListAsync());
        }

        // GET: HomepageBanners/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homepageBanner = await _context.HomepageBanner
                .Include(h => h.Member)
                .FirstOrDefaultAsync(m => m.BannerID == id);
            if (homepageBanner == null)
            {
                return NotFound();
            }

            return View(homepageBanner);
        }

        // GET: HomepageBanners/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.Member, "MemberID", "MemberID");
            return View();
        }

        // POST: HomepageBanners/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BannerID,ImageUrl,Title,Subtitle,Status,CreatedAt,CreatedBy")] HomepageBanner homepageBanner)
        {
            if (ModelState.IsValid)
            {
                _context.Add(homepageBanner);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.Member, "MemberID", "MemberID", homepageBanner.CreatedBy);
            return View(homepageBanner);
        }

        // GET: HomepageBanners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homepageBanner = await _context.HomepageBanner.FindAsync(id);
            if (homepageBanner == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.Member, "MemberID", "MemberID", homepageBanner.CreatedBy);
            return View(homepageBanner);
        }

        // POST: HomepageBanners/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BannerID,ImageUrl,Title,Subtitle,Status,CreatedAt,CreatedBy")] HomepageBanner homepageBanner)
        {
            if (id != homepageBanner.BannerID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(homepageBanner);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HomepageBannerExists(homepageBanner.BannerID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.Member, "MemberID", "MemberID", homepageBanner.CreatedBy);
            return View(homepageBanner);
        }

        // GET: HomepageBanners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var homepageBanner = await _context.HomepageBanner
                .Include(h => h.Member)
                .FirstOrDefaultAsync(m => m.BannerID == id);
            if (homepageBanner == null)
            {
                return NotFound();
            }

            return View(homepageBanner);
        }

        // POST: HomepageBanners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var homepageBanner = await _context.HomepageBanner.FindAsync(id);
            if (homepageBanner != null)
            {
                _context.HomepageBanner.Remove(homepageBanner);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HomepageBannerExists(int id)
        {
            return _context.HomepageBanner.Any(e => e.BannerID == id);
        }
    }
}

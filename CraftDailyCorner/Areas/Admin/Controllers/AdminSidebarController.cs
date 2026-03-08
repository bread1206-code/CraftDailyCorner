using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "03,04")]
    public class AdminSidebarController : Controller
    {
        private readonly IAdminSidebarService _sidebarService;

        public AdminSidebarController(IAdminSidebarService sidebarService)
        {
            _sidebarService = sidebarService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSidebarData()
        {
            var data = await _sidebarService.GetSidebarDataAsync();

            return Json(new
            {
                success = true,
                data
            });
        }
    }
}
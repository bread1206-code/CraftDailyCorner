using CraftDailyCorner.Areas.Admin.ViewModels;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CraftDailyCorner.Areas.Admin.ViewComponents
{
    public class VCAdminSidebar : ViewComponent
    {
        private readonly IAdminSidebarService _sidebarService;

        public VCAdminSidebar(IAdminSidebarService sidebarService)
        {
            _sidebarService = sidebarService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = await _sidebarService.GetSidebarDataAsync();
            return View(vm);
        }
    }
}
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Creator;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "02")]
public class PortfolioItemController : Controller
{
    private readonly ICreatorPortfolioItemService _service;
    private readonly ICreatorPortfolioService _creatorPortfolioService;

    public PortfolioItemController(
        ICreatorPortfolioItemService service,
        ICreatorPortfolioService creatorPortfolioService)
    {
        _service = service;
        _creatorPortfolioService = creatorPortfolioService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        string portfolioId,
        List<IFormFile> files)
    {
        await _service.UploadAsync(portfolioId,User.GetCreatorId(),files);

        return RedirectToAction("Edit", "Portfolio", new { id = portfolioId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int itemId)
    {
        var portfolioId = await _service.DeleteAsync(
            itemId,
            User.GetCreatorId());

        var vm = await _creatorPortfolioService
            .GetEditDataAsync(portfolioId, User.GetCreatorId());

        return PartialView("_PortfolioItemList", vm);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateSort(
        int itemId,
        byte sortOrder)
    {
        await _service.UpdateSortAsync(itemId,sortOrder,User.GetCreatorId());

        return Ok();
    }
}
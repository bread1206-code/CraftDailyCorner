using CraftDailyCorner.DTOs;
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
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        try
        {
            await _service.UploadAsync(
                portfolioId,
                creatorId,
                files);

            var vm = await _creatorPortfolioService.GetEditDataAsync(portfolioId, creatorId);
            if (vm == null)
                return NotFound();

            return PartialView("_PortfolioItemListPartial", vm);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int itemId)
    {
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        var portfolioId = await _service.DeleteAsync(itemId, creatorId);

        var vm = await _creatorPortfolioService.GetEditDataAsync(portfolioId, creatorId);
        if (vm == null)
            return NotFound();

        return PartialView("_PortfolioItemListPartial", vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSort([FromBody] List<SortUpdateDTO> items)
    {
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        await _service.UpdateSortBatchAsync(items, creatorId);

        return Ok();
    }
}
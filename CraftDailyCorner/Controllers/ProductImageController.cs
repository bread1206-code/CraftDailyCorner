using CraftDailyCorner.DTOs;
using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "02")]
public class ProductImageController : Controller
{
    private readonly CreatorProductImageService _service;

    public ProductImageController(CreatorProductImageService service)
    {
        _service = service;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        string productId,
        List<IFormFile> files)
    {
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        await _service.UploadAsync(
            productId,
            creatorId,
            files);

        var images = await _service.GetImagesAsync(
            productId,
            creatorId);

        return PartialView("_ProductImageListPartial", images);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long imageId)
    {
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        var productId = await _service.DeleteAsync(imageId, creatorId);

        var images = await _service.GetImagesAsync(productId, creatorId);

        return PartialView("_ProductImageListPartial", images);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSort(
        [FromBody] List<ImageSortDTO> items)
    {
        var creatorId = User.GetCreatorId();
        if (string.IsNullOrWhiteSpace(creatorId))
            return Unauthorized();

        await _service.UpdateSortBatchAsync(items, creatorId);

        return Ok();
    }
}
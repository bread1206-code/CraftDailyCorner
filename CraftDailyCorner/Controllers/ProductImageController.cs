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
        await _service.UploadAsync(
            productId,
            User.GetCreatorId(),
            files);

        var images = await _service.GetImagesAsync(
            productId,
            User.GetCreatorId());

        return PartialView("_ProductImageListPartial", images);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(long imageId)
    {
        var creatorId = User.GetCreatorId();

        var productId = await _service.DeleteAsync(imageId, creatorId);

        var images = await _service
            .GetImagesAsync(productId, creatorId);

        return PartialView("_ProductImageListPartial", images);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSort(
        [FromBody] List<ImageSortDTO> items)
    {
        await _service.UpdateSortBatchAsync(
            items,
            User.GetCreatorId());

        return Ok();
    }
}
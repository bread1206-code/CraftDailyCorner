using CraftDailyCorner.Extensions;
using CraftDailyCorner.ImageManagementCore.Interfaces;
using CraftDailyCorner.ImageManagementCore.Services.Interfaces;
using CraftDailyCorner.ImageManagementCore.ViewModels;
using CraftDailyCorner.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "02")]
[Route("ImageManagement")]
public class ImageManagementController : Controller
{
    private readonly Dictionary<string, IImageManagementService> _services;

    public ImageManagementController(
        IEnumerable<IImageManagementService> services)
    {
        _services = services
            .ToDictionary(s => s.EntityType, s => s);
    }

    private IImageManagementService GetService(string entityType)
    {
        if (!_services.TryGetValue(entityType, out var service))
            throw new Exception("未知圖片類型");

        return service;
    }

    // =============================
    // Upload
    // =============================

    [HttpPost("Upload")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Upload(
        string entityId,
        string entityType,
        List<IFormFile> files)
    {
        var creatorId = User.GetCreatorId();

        var service = GetService(entityType);

        foreach (var file in files)
        {
            await service.AddWithUploadAsync(
                file,
                entityId,
                creatorId);
        }

        return await ReloadPartial(entityId, entityType);
    }

    // =============================
    // Delete
    // =============================

    [HttpPost("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(
    string entityId,
    string entityType,
    long imageId)
    {
        try
        {
            var creatorId = User.GetCreatorId();
            var service = GetService(entityType);

            await service.DeleteWithValidationAsync(
                imageId,
                creatorId);

            return await ReloadPartial(entityId, entityType);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    // =============================
    // Sort
    // =============================

    [HttpPost("UpdateSort")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateSort(
    string entityId,
    string entityType,
    List<long> orderedIds)
    {
        var creatorId = User.GetCreatorId();

        var service = GetService(entityType);

        await service.UpdateSortWithValidationAsync(
            entityId,
            orderedIds,
            creatorId);

        return Ok();
    }

    // 商品
    [HttpGet("ProductPartial/{productId}")]
    public async Task<IActionResult> ProductPartial(string productId)
    {
        return await ReloadPartial(productId, "Product");
    }

    // 作品集
    [HttpGet("PortfolioPartial/{portfolioId}")]
    public async Task<IActionResult> PortfolioPartial(string portfolioId)
    {
        return await ReloadPartial(portfolioId, "Portfolio");
    }

    // =============================
    // Reload Partial
    // =============================

    private async Task<IActionResult> ReloadPartial(
        string entityId,
        string entityType)
    {
        var service = GetService(entityType);

        var images = await service.GetImagesAsync(entityId);

        var vm = new VMImageManagement
        {
            EntityId = entityId,
            EntityType = entityType,
            MaxImageCount = service.MaxImageCount,
            HintMessage = service.HintMessage,
            Images = images.Select(x => new VMImageItem
            {
                ImageId = x.ImageID,
                ImageUrl = BuildImageUrl(x),
                SortOrder = x.SortOrder
            }).ToList()
        };

        return PartialView("_ImageManagementPartial", vm);
    }

    private string BuildImageUrl(IEntityImage image)
    {
        if (image is ProductImage)
            return $"/Photos/04ProductImage/Medium/{image.ImageUrl}.png";

        if (image is PortfolioItem)
            return $"/Photos/06Portfolio/Medium/{image.ImageUrl}.png";

        throw new Exception("未知圖片類型");
    }
}
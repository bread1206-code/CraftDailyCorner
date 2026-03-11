using CraftDailyCorner.Extensions;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorOrder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "02")]
public class CreatorOrdersController : Controller
{
    private readonly ICreatorOrderService _orderService;
    private readonly ICreatorPickListService _pickListService;
    private readonly ICreatorShipmentService _shipmentService;

    public CreatorOrdersController(
        ICreatorOrderService orderService,
        ICreatorPickListService pickListService,
        ICreatorShipmentService shipmentService)
    {
        _orderService = orderService;
        _pickListService = pickListService;
        _shipmentService = shipmentService;
    }

    private string CreatorId => User.GetCreatorId();

    public async Task<IActionResult> Index(string status = "new", int page = 1)
    {
        var vm = await _orderService.GetOrdersAsync(CreatorId, status, page);
        return View(vm);
    }

    public async Task<IActionResult> Detail(string id)
    {
        var vm = await _orderService.GetOrderDetailAsync(CreatorId, id);

        if (vm == null)
            return NotFound();

        if (vm.StatusID == 3)
        {
            vm.SuggestedTrackingNo = await _shipmentService.GenerateTrackingNoAsync();
        }

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> StartProcessing(string id)
    {
        var success = await _orderService.StartProcessingAsync(CreatorId, id);

        if (!success)
            return NotFound();

        return RedirectToAction(nameof(Index), new { status = "processing" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ship(string orderId, string trackingNo)
    {
        if (string.IsNullOrWhiteSpace(trackingNo))
        {
            TempData["Error"] = "請輸入物流編號";
            return RedirectToAction(nameof(Detail), new { id = orderId });
        }

        var result = await _orderService.ShipAndGetNextAsync(CreatorId, orderId, trackingNo);

        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage ?? "出貨失敗";
            return RedirectToAction(nameof(Detail), new { id = orderId });
        }

        if (!string.IsNullOrEmpty(result.NextOrderId))
            return RedirectToAction(nameof(Detail), new { id = result.NextOrderId });

        return RedirectToAction(nameof(Index), new { status = "shipping" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkDelivered(string orderId)
    {
        var success = await _orderService.MarkDeliveredAsync(CreatorId, orderId);

        TempData[success ? "Success" : "Error"] = success
            ? "已更新為商品送達"
            : "商品送達更新失敗";

        return RedirectToAction(nameof(Detail), new { id = orderId });
    }

    [HttpPost]
    public async Task<IActionResult> BatchPrint(List<string> SelectedOrderIDs)
    {
        var vm = await _pickListService.GeneratePickListPreviewAsync(CreatorId, SelectedOrderIDs);

        if (vm == null)
            return RedirectToAction(nameof(Index), new { status = "new" });

        TempData["PickListOrderIDs"] = string.Join(",", SelectedOrderIDs);

        return View("PickListPreview", vm);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmPrint()
    {
        if (TempData["PickListOrderIDs"] == null)
            return RedirectToAction(nameof(Index), new { status = "new" });

        var orderIds = TempData["PickListOrderIDs"]!
            .ToString()!
            .Split(',')
            .ToList();

        await _pickListService.ConfirmPrintAsync(CreatorId, orderIds);

        return RedirectToAction(nameof(Index), new { status = "processing" });
    }
}
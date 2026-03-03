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

    public CreatorOrdersController(ICreatorOrderService orderService, ICreatorPickListService pickListService, ICreatorShipmentService shipmentService)
    {
        _orderService = orderService;
        _pickListService = pickListService;
        _shipmentService = shipmentService;
    }

    private string CreatorId => User.GetCreatorId();

    // 訂單列表
    public async Task<IActionResult> Index(string status = "new", int page = 1)
    {
        var vm = await _orderService
            .GetOrdersAsync(CreatorId, status, page);

        return View(vm);
    }

    // 訂單明細
    public async Task<IActionResult> Detail(string id)
    {
        var vm = await _orderService
            .GetOrderDetailAsync(CreatorId, id);

        if (vm == null)
            return NotFound();
        if (vm.StatusID == 3) // Processing
        {
            vm.SuggestedTrackingNo =
                await _shipmentService.GenerateTrackingNoAsync();
        }

        return View(vm);
    }

    // 開始處理（Paid → Processing）
    [HttpPost]
    public async Task<IActionResult> StartProcessing(string id)
    {
        var success = await _orderService
            .StartProcessingAsync(CreatorId, id);

        if (!success)
            return NotFound();

        return RedirectToAction(nameof(Index), new { status = "processing" });
    }

    // 出貨（Processing → Shipped）
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Ship(string orderId, string trackingNo)
    {
        if (string.IsNullOrWhiteSpace(trackingNo))
        {
            TempData["Error"] = "請輸入物流編號";
            return RedirectToAction(nameof(Detail), new { id = orderId });
        }

        var result = await _orderService
            .ShipAndGetNextAsync(CreatorId, orderId, trackingNo);

        if (!result.Success)
        {
            TempData["Error"] = result.ErrorMessage ?? "出貨失敗";
            return RedirectToAction(nameof(Detail), new { id = orderId });
        }

        if (!string.IsNullOrEmpty(result.NextOrderId))
            return RedirectToAction(nameof(Detail),
                new { id = result.NextOrderId });

        return RedirectToAction(nameof(Index),
            new { status = "shipping" });
    }
    // 批次列印撿貨單（新訂單）
    [HttpPost]
    public async Task<IActionResult> BatchPrint(List<string> SelectedOrderIDs)
    {
        var vm = await _pickListService
            .GeneratePickListPreviewAsync(CreatorId, SelectedOrderIDs);

        if (vm == null)
            return RedirectToAction(nameof(Index), new { status = "new" });

        // 將選擇的ID暫存到TempData
        TempData["PickListOrderIDs"] =
            string.Join(",", SelectedOrderIDs);

        return View("PickListPreview", vm);
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmPrint()
    {
        if (TempData["PickListOrderIDs"] == null)
            return RedirectToAction(nameof(Index), new { status = "new" });

        var orderIds = TempData["PickListOrderIDs"]
            .ToString()
            .Split(',')
            .ToList();

        await _pickListService
            .ConfirmPrintAsync(CreatorId, orderIds);

        return RedirectToAction(nameof(Index), new { status = "processing" });
    }
}
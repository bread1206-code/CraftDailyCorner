using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorOrder;

public interface ICreatorOrderService
{
    Task<VMCreatorOrderList> GetOrdersAsync(string creatorId, string status, int page);

    Task<VMCreatorOrderDetail?> GetOrderDetailAsync(string creatorId, string orderId);

    Task<bool> StartProcessingAsync(string creatorId, string orderId);

    Task<bool> ShipAsync(string creatorId, VMCreatorShipmentUpdate model);
    Task<ShipResult> ShipAndGetNextAsync(string creatorId, string orderId, string trackingNo);

    Task<bool> MarkDeliveredAsync(string creatorId, string orderId);

    Task<(int NewCount, int ProcessingCount, int ShippingCount, int HistoryCount)> GetOrderStatisticsAsync(string creatorId);
}
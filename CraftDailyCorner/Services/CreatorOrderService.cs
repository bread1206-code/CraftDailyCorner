using CraftDailyCorner.Models;
using CraftDailyCorner.ViewModels.Creator;
using CraftDailyCorner.ViewModels.CreatorOrder;
using Microsoft.EntityFrameworkCore;

public class CreatorOrderService : ICreatorOrderService
{
    private readonly CraftDailyCornerContext _context;
    private const int PageSize = 20;

    public CreatorOrderService(CraftDailyCornerContext context)
    {
        _context = context;
    }

    // 訂單列表
    public async Task<VMCreatorOrderList> GetOrdersAsync(
        string creatorId,
        string status,
        int page)
    {
        var query = _context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.Shipment)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .Where(o => o.OrderDetails
                .Any(d => d.Product.CreatorID == creatorId));

        query = status.ToLower() switch
        {
            "new" => query.Where(o => o.StatusID == 2),
            "processing" => query.Where(o => o.StatusID == 3),
            "shipping" => query.Where(o => o.StatusID == 4),
            "history" => query.Where(o => o.StatusID == 5 || o.StatusID == 6),
            _ => query.Where(o => o.StatusID == 2)
        };

        int totalCount = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * PageSize)
            .Take(PageSize)
            .Select(o => new VMCreatorOrderItem
            {
                OrderID = o.OrderID,
                ReceiverName = o.ReceiverName,
                CreatedAt = o.CreatedAt,
                TotalAmount = o.TotalAmount,
                StatusID = o.StatusID,
                StatusName = o.OrderStatus.StatusName,
                HasShipment = o.Shipment != null,
                TrackingNo = o.Shipment != null ? o.Shipment.TrackingNo : null
            })
            .ToListAsync();

        return new VMCreatorOrderList
        {
            StatusFilter = status,
            Orders = orders,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize)
        };
    }

    //訂單明細
    public async Task<VMCreatorOrderDetail?> GetOrderDetailAsync(
        string creatorId,
        string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderStatus)
            .Include(o => o.Shipment)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.OrderID == orderId &&
                o.OrderDetails.Any(d => d.Product.CreatorID == creatorId));

        if (order == null)
            return null;

        return new VMCreatorOrderDetail
        {
            OrderID = order.OrderID,
            ReceiverName = order.ReceiverName,
            ReceiverPhone = order.ReceiverPhone,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            TotalAmount = order.TotalAmount,
            StatusID = order.StatusID,
            StatusName = order.OrderStatus.StatusName,
            TrackingNo = order.Shipment?.TrackingNo,
            Items = order.OrderDetails.Select(d => new VMCreatorOrderDetailItem
            {
                ProductID = d.ProductID,
                ProductNameSnapshot = d.ProductNameSnapshot,
                PriceSnapshot = d.PriceSnapshot,
                CostSnapshot = d.CostSnapshot,
                Quantity = d.Quantity
            }).ToList()
        };
    }

    // 開始處理
    public async Task<bool> StartProcessingAsync(
        string creatorId,
        string orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.OrderID == orderId &&
                o.StatusID == 2 &&
                o.OrderDetails.Any(d => d.Product.CreatorID == creatorId));

        if (order == null)
            return false;

        order.StatusID = 3;
        order.UpdatedAt = DateTime.Now;

        _context.Shipments.Add(new Shipment
        {
            OrderID = order.OrderID,
            StatusID = 1
        });

        await _context.SaveChangesAsync();
        return true;
    }

    //出貨
    public async Task<bool> ShipAsync(
        string creatorId,
        VMCreatorShipmentUpdate model)
    {
        var order = await _context.Orders
            .Include(o => o.Shipment)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o => o.OrderID == model.OrderID &&
                o.StatusID == 3 &&
                o.OrderDetails.Any(d => d.Product.CreatorID == creatorId));

        if (order == null)
            return false;

        order.StatusID = 4;
        order.UpdatedAt = DateTime.Now;

        if (order.Shipment != null)
        {
            order.Shipment.TrackingNo = model.TrackingNo;
            order.Shipment.StatusID = 2;
        }

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<ShipResult> ShipAndGetNextAsync(
    string creatorId,
    string orderId,
    string trackingNo)
    {
        //取得訂單
        var order = await _context.Orders
            .Include(o => o.Shipment)
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .FirstOrDefaultAsync(o =>
                o.OrderID == orderId &&
                o.StatusID == 3 &&
                o.OrderDetails.Any(d =>
                    d.Product.CreatorID == creatorId));

        if (order == null)
            return new ShipResult { Success = false };

        //更新為 Shipped
        order.StatusID = 4;
        order.UpdatedAt = DateTime.Now;

        if (order.Shipment == null)
        {
            order.Shipment = new Shipment
            {
                TrackingNo = trackingNo,
                StatusID = 2,
                OrderID = order.OrderID
            };
        }
        else
        {
            order.Shipment.TrackingNo = trackingNo;
            order.Shipment.StatusID = 2;
        }

        await _context.SaveChangesAsync();

        //查下一張 Processing
        var nextOrderId = await _context.Orders
            .Include(o => o.OrderDetails)
                .ThenInclude(d => d.Product)
            .Where(o =>
                o.StatusID == 3 &&
                o.CreatedAt < order.CreatedAt &&
                o.OrderDetails.Any(d =>
                    d.Product.CreatorID == creatorId))
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => o.OrderID)
            .FirstOrDefaultAsync();

        return new ShipResult
        {
            Success = true,
            NextOrderId = nextOrderId
        };
    }
    //取得訂單統計
    public async Task<(int NewCount, int ProcessingCount, int ShippingCount, int HistoryCount)>
    GetOrderStatisticsAsync(string creatorId)
    {
        var grouped = await _context.Orders
            .Where(o => o.OrderDetails
                .Any(d => d.Product.CreatorID == creatorId))
            .GroupBy(o => o.StatusID)
            .Select(g => new
            {
                StatusID = g.Key,
                Count = g.Count()
            })
            .ToListAsync();

        int newCount = grouped.FirstOrDefault(x => x.StatusID == 2)?.Count ?? 0;
        int processingCount = grouped.FirstOrDefault(x => x.StatusID == 3)?.Count ?? 0;
        int shippingCount = grouped.FirstOrDefault(x => x.StatusID == 4)?.Count ?? 0;
        int historyCount =
            grouped.Where(x => x.StatusID == 5 || x.StatusID == 6)
                   .Sum(x => x.Count);

        return (newCount, processingCount, shippingCount, historyCount);
    }
}
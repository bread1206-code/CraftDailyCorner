using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorPickList;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class CreatorPickListService : ICreatorPickListService
    {
        private readonly CraftDailyCornerContext _context;

        public CreatorPickListService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        public async Task<VMCreatorPickList?> GeneratePickListPreviewAsync(string creatorId, List<string> orderIds)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .Where(o =>
                    orderIds.Contains(o.OrderID) &&
                    o.StatusID == 2 &&
                    o.OrderDetails != null &&
                    o.OrderDetails.Any(d => d.Product.CreatorID == creatorId))
                .ToListAsync();

            if (!orders.Any())
                return null;

            var summary = orders
                .SelectMany(o => o.OrderDetails ?? Enumerable.Empty<OrderDetail>())
                .GroupBy(d => new { d.ProductID, d.ProductNameSnapshot })
                .Select(g => new VMPickListSummaryItem
                {
                    ProductID = g.Key.ProductID,
                    ProductName = g.Key.ProductNameSnapshot,
                    TotalQuantity = g.Sum(x => x.Quantity)
                })
                .ToList();

            return new VMCreatorPickList
            {
                Orders = orders.Select(o => new VMPickListOrder
                {
                    OrderID = o.OrderID,
                    ReceiverName = o.ReceiverName,
                    ReceiverPhone = o.ReceiverPhone,
                    ShippingAddress = o.ShippingAddress,
                    CreatedAt = o.CreatedAt,
                    Items = (o.OrderDetails ?? Enumerable.Empty<OrderDetail>())
                        .Select(d => new VMPickListOrderItem
                        {
                            ProductID = d.ProductID,
                            ProductName = d.ProductNameSnapshot,
                            Quantity = d.Quantity
                        })
                        .ToList()
                }).ToList(),

                SummaryItems = summary,
                TotalOrderCount = orders.Count
            };
        }

        public async Task<bool> ConfirmPrintAsync(string creatorId, List<string> orderIds)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderDetails)
                    .ThenInclude(d => d.Product)
                .Where(o =>
                    orderIds.Contains(o.OrderID) &&
                    o.StatusID == 2 &&
                    o.OrderDetails != null &&
                    o.OrderDetails.Any(d => d.Product.CreatorID == creatorId))
                .ToListAsync();

            if (!orders.Any())
                return false;

            foreach (var order in orders)
            {
                order.StatusID = 3;
                order.UpdatedAt = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
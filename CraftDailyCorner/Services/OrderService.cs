using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.Member;
using CraftDailyCorner.ViewModels.Order;
using CraftDailyCorner.ViewModels.Payment;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CraftDailyCorner.Services
{
    public class OrderService
    {
        private readonly CraftDailyCornerContext _context;

        public OrderService(CraftDailyCornerContext context)
        {
            _context = context;
        }

        // 建立訂單
        public VMCreateOrderResult CreateOrder(string memberId, VMCreateOrderRequest request)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var now = DateTime.Now;

                var selectedProductIds = request.SelectedProductIds?
                    .Where(id => !string.IsNullOrWhiteSpace(id))
                    .Distinct()
                    .ToList() ?? new List<string>();

                if (!selectedProductIds.Any())
                {
                    return new VMCreateOrderResult
                    {
                        Success = false,
                        Message = "請先勾選要結帳的商品"
                    };
                }

                var cartItems = _context.CartItems
                    .Include(ci => ci.Cart)
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.Inventory)
                    .Include(ci => ci.Product)
                        .ThenInclude(p => p.CreatorProfile)
                    .Where(ci =>
                        ci.Cart.MemberID == memberId &&
                        ci.Product.CreatorID == request.CreatorId &&
                        selectedProductIds.Contains(ci.ProductID))
                    .ToList();

                if (!cartItems.Any())
                {
                    return new VMCreateOrderResult
                    {
                        Success = false,
                        Message = "購物車內無本次勾選的商品"
                    };
                }

                if (cartItems.Count != selectedProductIds.Count)
                {
                    return new VMCreateOrderResult
                    {
                        Success = false,
                        Message = "部分勾選商品不存在，請重新確認購物車內容"
                    };
                }

                var orderId = GetNewOrderID();
                var totalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

                var order = new Order
                {
                    OrderID = orderId,
                    MemberID = memberId,
                    CreatedAt = now,
                    ReceiverName = request.ReceiverName,
                    ReceiverPhone = request.ReceiverPhone,
                    ShippingAddress = request.ReceiverAddress,
                    TotalAmount = totalAmount,
                    StatusID = 1 // 待付款
                };

                _context.Orders.Add(order);

                var lowStockCreatorDtos = new List<CreateNotificationDTO>();
                var outOfStockCreatorDtos = new List<CreateNotificationDTO>();

                foreach (var item in cartItems)
                {
                    var product = item.Product;
                    var inventory = product.Inventory;

                    if (inventory == null || inventory.StockQty < item.Quantity)
                    {
                        throw new Exception($"商品「{product.ProductName}」庫存不足（僅剩 {inventory?.StockQty ?? 0} 件）");
                    }

                    var oldStock = inventory.StockQty;
                    inventory.StockQty -= item.Quantity;
                    inventory.UpdatedAt = now;

                    var newStock = inventory.StockQty;
                    var alertQty = inventory.AlertQty;

                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = product.ProductID,
                        ProductNameSnapshot = product.ProductName,
                        PriceSnapshot = product.Price,
                        CostSnapshot = product.CostPrice,
                        Quantity = item.Quantity
                    };
                    _context.OrderDetails.Add(orderDetail);

                    var creatorMemberId = product.CreatorProfile?.MemberID;
                    if (!string.IsNullOrWhiteSpace(creatorMemberId))
                    {
                        if (oldStock > alertQty && newStock > 0 && newStock <= alertQty)
                        {
                            lowStockCreatorDtos.Add(new CreateNotificationDTO
                            {
                                MemberID = creatorMemberId,
                                NotificationType = NotificationType.ProductLowStock,
                                Title = "商品低庫存通知",
                                Content = $"商品「{product.ProductName}」目前庫存僅剩 {newStock} 件，已達警戒值。",
                                LinkUrl = $"/CreatorProducts/Edit/{product.ProductID}",
                                RelatedEntityType = "Product",
                                RelatedEntityId = product.ProductID
                            });
                        }

                        if (oldStock > 0 && newStock == 0)
                        {
                            outOfStockCreatorDtos.Add(new CreateNotificationDTO
                            {
                                MemberID = creatorMemberId,
                                NotificationType = NotificationType.ProductOutOfStock,
                                Title = "商品缺貨通知",
                                Content = $"商品「{product.ProductName}」目前已缺貨。",
                                LinkUrl = $"/CreatorProducts/Edit/{product.ProductID}",
                                RelatedEntityType = "Product",
                                RelatedEntityId = product.ProductID
                            });
                        }
                    }
                }

                _context.CartItems.RemoveRange(cartItems);

                _context.SaveChanges();

                // ===== 會員：訂單已成立 =====
                _context.NotificationEvents.Add(new NotificationEvent
                {
                    MemberID = memberId,
                    NotificationType = NotificationType.OrderCreated,
                    Title = "訂單已成立通知",
                    Content = $"訂單 {order.OrderID} 已成立，請前往完成付款。",
                    LinkUrl = $"/Orders/Detail?orderId={order.OrderID}",
                    IsRead = false,
                    RelatedEntityType = "Order",
                    RelatedEntityId = order.OrderID,
                    CreatedAt = now
                });

                // ===== 創作者：低庫存 =====
                foreach (var dto in lowStockCreatorDtos)
                {
                    _context.NotificationEvents.Add(new NotificationEvent
                    {
                        MemberID = dto.MemberID,
                        NotificationType = dto.NotificationType,
                        Title = dto.Title,
                        Content = dto.Content,
                        LinkUrl = dto.LinkUrl,
                        IsRead = false,
                        RelatedEntityType = dto.RelatedEntityType,
                        RelatedEntityId = dto.RelatedEntityId,
                        CreatedAt = now
                    });
                }

                // ===== 創作者：缺貨 =====
                foreach (var dto in outOfStockCreatorDtos)
                {
                    _context.NotificationEvents.Add(new NotificationEvent
                    {
                        MemberID = dto.MemberID,
                        NotificationType = dto.NotificationType,
                        Title = dto.Title,
                        Content = dto.Content,
                        LinkUrl = dto.LinkUrl,
                        IsRead = false,
                        RelatedEntityType = dto.RelatedEntityType,
                        RelatedEntityId = dto.RelatedEntityId,
                        CreatedAt = now
                    });
                }

                _context.SaveChanges();

                transaction.Commit();

                return new VMCreateOrderResult
                {
                    Success = true,
                    Message = "訂單建立成功",
                    OrderID = order.OrderID
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new VMCreateOrderResult
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        // 我的訂單列表
        public List<VMMyOrder> GetMyOrders(string memberId, string? statusCode)
        {
            var query = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderStatus)
                .Where(o => o.MemberID == memberId);

            query = statusCode switch
            {
                "padding" =>
                    query.Where(o => o.OrderStatus.StatusID == 1),

                "processing" =>
                    query.Where(o => o.OrderStatus.StatusID == 2 || o.OrderStatus.StatusID == 3 || o.OrderStatus.StatusID == 4),

                _ => query
            };

            return query
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new VMMyOrder
                {
                    OrderID = o.OrderID,
                    CreatedAt = o.CreatedAt,
                    TotalAmount = (int)Math.Floor(o.TotalAmount),
                    StatusText = o.OrderStatus.StatusName,
                    UpdatedAt = o.UpdatedAt
                })
                .ToList();
        }

        // 我的訂單詳細內容
        public VMMyOrderDetail? GetOrderDetail(string orderId, string memberId)
        {
            var order = _context.Orders
                .AsNoTracking()
                .Include(o => o.OrderStatus)
                .Include(o => o.Shipment)
                    .ThenInclude(s => s.ShipmentStatus)
                .Include(o => o.OrderDetails!)
                    .ThenInclude(od => od.Product)
                        .ThenInclude(p => p.CreatorProfile)
                .FirstOrDefault(o =>
                    o.OrderID == orderId &&
                    o.MemberID == memberId);

            if (order == null)
                return null;

            var payments = _context.Payments
                .Where(p => p.OrderID == orderId)
                .OrderBy(p => p.CreatedAt)
                .Select(p => new VMOrderPaymentItem
                {
                    Amount = p.Amount,
                    MethodName = p.PaymentMethod.MethodName,
                    StatusName = p.PaymentStatus.StatusName,
                    CreatedAt = p.CreatedAt,
                    PaidAt = p.PaidAt
                })
                .ToList();

            return new VMMyOrderDetail
            {
                OrderID = order.OrderID,
                CreatedAt = order.CreatedAt,
                TotalAmount = (int)Math.Floor(order.TotalAmount),
                StatusText = order.OrderStatus.StatusName,
                StatusID = order.OrderStatus.StatusID,

                ReceiverName = order.ReceiverName,
                ReceiverPhone = order.ReceiverPhone,
                ShippingAddress = order.ShippingAddress,

                Items = order.OrderDetails!
                    .Select(od => new VMMyOrderItem
                    {
                        ProductID = od.ProductID,
                        ProductName = od.ProductNameSnapshot,
                        Price = (int)Math.Floor(od.PriceSnapshot),
                        Quantity = od.Quantity,
                        BrandName = od.Product.CreatorProfile!.BrandName
                    })
                    .ToList(),

                OrderPayments = new VMOrderPaymentList
                {
                    OrderID = order.OrderID,
                    Payments = payments
                },

                ShipmentStatusID = order.Shipment?.StatusID,
                TrackingNo = order.Shipment?.TrackingNo,
                ShippedAt = order.Shipment?.ShippedAt,
                DeliveredAt = order.Shipment?.DeliveredAt
            };
        }

        private string GetNewOrderID()
        {
            var outputParam = new SqlParameter
            {
                ParameterName = "@NewOrderID",
                SqlDbType = SqlDbType.NChar,
                Size = 12,
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC getCreatedOrderID @NewOrderID OUTPUT",
                outputParam
            );

            return outputParam.Value!.ToString()!;
        }

        // 取消訂單
        public (bool Success, string Message) CancelOrder(string orderId, string memberId)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var order = _context.Orders
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.Inventory)
                    .FirstOrDefault(o => o.OrderID == orderId && o.MemberID == memberId);

                if (order == null)
                    return (false, "找不到該訂單");

                if (order.StatusID != 1)
                {
                    return (false, "目前訂單狀態不可取消（僅限待付款訂單）");
                }

                if (order.OrderDetails != null)
                {
                    foreach (var detail in order.OrderDetails)
                    {
                        if (detail.Product.Inventory != null)
                        {
                            detail.Product.Inventory.StockQty += detail.Quantity;
                            detail.Product.Inventory.UpdatedAt = DateTime.Now;
                        }
                    }
                }

                order.StatusID = 6;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();
                transaction.Commit();

                return (true, "訂單已成功取消，庫存已釋出");
            }
            catch
            {
                transaction.Rollback();
                return (false, "系統錯誤，無法取消訂單");
            }
        }

        // 會員確認取貨（完成訂單）
        public (bool Success, string Message) ConfirmPickup(string orderId, string memberId)
        {
            using var tx = _context.Database.BeginTransaction();

            try
            {
                var order = _context.Orders
                    .Include(o => o.Shipment)
                    .Include(o => o.OrderDetails)
                        .ThenInclude(od => od.Product)
                            .ThenInclude(p => p.CreatorProfile)
                    .FirstOrDefault(o => o.OrderID == orderId && o.MemberID == memberId);

                if (order == null)
                    return (false, "找不到該訂單");

                if (order.StatusID == 6)
                    return (false, "此訂單已取消");

                if (order.StatusID != 4)
                    return (false, "目前訂單狀態不可取貨（僅限配送中）");

                if (order.Shipment == null)
                    return (false, "找不到物流資訊，無法取貨");

                if (order.Shipment.StatusID != 3)
                    return (false, "商品尚未送達，無法取貨");

                order.StatusID = 5;
                order.UpdatedAt = DateTime.Now;

                _context.SaveChanges();

                // ===== 創作者：訂單完成通知 =====
                var creatorMemberIds = order.OrderDetails
                    .Where(x => x.Product?.CreatorProfile != null)
                    .Select(x => x.Product.CreatorProfile!.MemberID)
                    .Distinct()
                    .ToList();

                foreach (var creatorMemberId in creatorMemberIds)
                {
                    _context.NotificationEvents.Add(new NotificationEvent
                    {
                        MemberID = creatorMemberId,
                        NotificationType = NotificationType.OrderCompleted,
                        Title = "訂單完成通知",
                        Content = $"訂單 {order.OrderID} 已完成。",
                        LinkUrl = $"/CreatorOrders/Detail?id={order.OrderID}",
                        IsRead = false,
                        RelatedEntityType = "Order",
                        RelatedEntityId = order.OrderID,
                        CreatedAt = DateTime.Now
                    });
                }

                _context.SaveChanges();
                tx.Commit();

                return (true, "已完成取貨，訂單已完成");
            }
            catch
            {
                tx.Rollback();
                return (false, "系統錯誤，取貨失敗");
            }
        }

    }
}